using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Repository;
using Slnfgen.CLI.Domain.Solution.File.Repository;
using Slnfgen.CLI.Domain.Solution.File.Services;
using Slnfgen.CLI.Domain.Solution.Filter.Repository;
using Slnfgen.CLI.Domain.Solution.Filter.Services;

namespace Slnfgen.CLI.Application.Requests.GenerateAll;

/// <summary>
///     Generate a Solution Filter file
/// </summary>
public class GenerateSolutionFiltersRequestHandler
    : IRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse>
{
    private readonly ILogger<GenerateSolutionFiltersRequestHandler> _logger;
    private readonly SolutionFilterGenerator _solutionFilterGenerator;
    private readonly ISolutionFilterWriter _solutionFilterWriter;
    private readonly ISolutionLoader _solutionLoader;
    private readonly ISolutionFiltersManifestLoader _solutionManifestLoader;

    /// <inheritdoc cref="GenerateSolutionFiltersRequestHandler" />
    public GenerateSolutionFiltersRequestHandler(
        SolutionFilterGenerator solutionFilterGenerator,
        ISolutionFilterWriter solutionFilterWriter,
        IXmlSolutionFileWriter solutionFileWriter,
        ISolutionFiltersManifestLoader solutionManifestLoader,
        ISolutionLoader solutionLoader,
        ILogger<GenerateSolutionFiltersRequestHandler> logger,
        SolutionGenerator solutionGenerator
    )
    {
        _solutionFilterGenerator = solutionFilterGenerator;
        _solutionFilterWriter = solutionFilterWriter;
        _logger = logger;
        _solutionManifestLoader = solutionManifestLoader;
        _solutionLoader = solutionLoader;
    }

    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    public GenerateSolutionFiltersResponse Handle(GenerateSolutionFiltersRequest request)
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

        _logger.LogInformation("Generating solution filters");
        var solutionFilters = _solutionFilterGenerator
            .GenerateMany(solutionFile, filtersDefinition, request.OutputDirectory)
            .ToList();
        _logger.LogInformation("Generated {Count} solution filters", solutionFilters.Count);

        if (request.DryRun)
            return new GenerateSolutionFiltersResponse(solutionFilters.Select(slnf => slnf.GetFileName()));

        var storedFilters = _solutionFilterWriter.WriteMany(solutionFilters, request.OutputDirectory);
        _logger.LogInformation("Solution filters written to: {OutputDirectory}", request.OutputDirectory);
        return new GenerateSolutionFiltersResponse(storedFilters);
    }
}
