using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Repository;
using Slnfgen.CLI.Domain.Solution.File.Repository;
using Slnfgen.CLI.Domain.Solution.Filter.Repository;
using Slnfgen.CLI.Domain.Solution.Filter.Services;

namespace Slnfgen.CLI.Application.Requests.GenerateTarget;

/// <summary>
///     Request handler for generating a Solution Filter file for a specific target defined in the manifest file.
///     This handler processes the request to generate a solution filter based on the provided target,
///     solution file, and filters configuration.
/// </summary>
public class GenerateTargetSolutionFilterRequestHandler
    : IRequestHandler<GenerateTargetSolutionFilterRequest, GenerateTargetSolutionFilterResponse>
{
    private readonly ILogger<GenerateSolutionFiltersRequestHandler> _logger;
    private readonly SolutionFilterGenerator _solutionFilterGenerator;
    private readonly ISolutionFilterWriter _solutionFilterWriter;
    private readonly ISolutionLoader _solutionLoader;
    private readonly ISolutionFiltersManifestLoader _solutionManifestLoader;

    /// <inheritdoc cref="GenerateSolutionFiltersRequestHandler" />
    public GenerateTargetSolutionFilterRequestHandler(
        SolutionFilterGenerator solutionFilterGenerator,
        ISolutionFilterWriter solutionFilterWriter,
        ISolutionFiltersManifestLoader solutionManifestLoader,
        ISolutionLoader solutionLoader,
        ILogger<GenerateSolutionFiltersRequestHandler> logger
    )
    {
        _solutionFilterGenerator = solutionFilterGenerator;
        _solutionFilterWriter = solutionFilterWriter;
        _logger = logger;
        _solutionManifestLoader = solutionManifestLoader;
        _solutionLoader = solutionLoader;
    }

    /// <summary>
    ///     Handles the request to generate a solution filter for a specific target.
    /// </summary>
    /// <param name="request"></param>
    public GenerateTargetSolutionFilterResponse Handle(GenerateTargetSolutionFilterRequest request)
    {
        var filtersDefinition = _solutionManifestLoader.Load(request.ManifestFilePath);
        _logger.LogInformation(
            "Loaded filters file '{Path}' for Solution file '{SolutionFile}'",
            request.ManifestFilePath,
            filtersDefinition.SolutionFile
        );

        var solutionFilePath = Path.Combine(
            Directory.GetParent(request.ManifestFilePath)!.FullName,
            filtersDefinition.SolutionFile
        );
        var solutionFile = _solutionLoader.Load(solutionFilePath);
        _logger.LogInformation("Loaded solution file: {Path}", solutionFile.FileName);

        _logger.LogInformation("Generating solution filter: {Target}", request.Target);
        var solutionFilter = _solutionFilterGenerator.GenerateForTarget(
            request.Target,
            solutionFile,
            filtersDefinition,
            request.OutputDirectory
        );
        _logger.LogInformation("Generated solution filter: {Target}", solutionFilter.GetFileName());

        if (request.DryRun)
            return new GenerateTargetSolutionFilterResponse(solutionFilter.GetFileName());
        var generatedFilters = _solutionFilterWriter.Write(solutionFilter, request.OutputDirectory);
        return new GenerateTargetSolutionFilterResponse(generatedFilters);
    }
}
