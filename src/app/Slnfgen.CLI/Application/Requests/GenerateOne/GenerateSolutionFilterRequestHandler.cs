using Microsoft.Extensions.Logging;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;
using Slnfgen.CLI.Application.Services.SolutionFilter;
using Slnfgen.CLI.Domain.Solution.File.Loader;

namespace Slnfgen.CLI.Application.Requests.GenerateOne;

/// <summary>
///
/// </summary>
public class GenerateSolutionFilterRequestHandler
    : IRequestHandler<GenerateSolutionFilterRequest, GenerateSolutionFilterResponse>
{
    private readonly ILogger<GenerateSolutionFiltersRequestHandler> _logger;
    private readonly SolutionFilterGenerator _solutionFilterGenerator;
    private readonly ISolutionFilterWriter _solutionFilterWriter;
    private readonly ISolutionLoader _solutionLoader;
    private readonly ISolutionFiltersManifestLoader _solutionManifestLoader;

    /// <inheritdoc cref="GenerateSolutionFiltersRequestHandler" />
    public GenerateSolutionFilterRequestHandler(
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
    /// </summary>
    /// <param name="request"></param>
    public GenerateSolutionFilterResponse Handle(GenerateSolutionFilterRequest request)
    {
        var filtersDefinition = _solutionManifestLoader.Load(request.FiltersConfigFilePath);
        _logger.LogInformation(
            "Loaded filters file '{Path}' for Solution file '{SolutionFile}'",
            request.FiltersConfigFilePath,
            filtersDefinition.SolutionFile
        );

        var solutionFilePath = Path.Combine(
            Directory.GetParent(request.FiltersConfigFilePath)!.FullName,
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
            return new GenerateSolutionFilterResponse(solutionFilter.GetFileName());
        var generatedFilters = _solutionFilterWriter.Write(solutionFilter, request.OutputDirectory);
        return new GenerateSolutionFilterResponse(generatedFilters);
    }
}
