using Microsoft.Extensions.Logging;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Domain.Solution.File.Loader;

namespace Slnfgen.CLI.Application.Services.SolutionFilter;

/// <summary>
///
/// </summary>
public class SolutionFilterFileGenerationHandler : IRequestHandler<GenerateSolutionFiltersRequest>
{
    private readonly SolutionFilterGenerator _solutionFilterGenerator;
    private readonly ISolutionFilterWriter _solutionFilterWriter;
    private readonly ISolutionFiltersManifestLoader _solutionFiltersConfigurationLoader;
    private readonly ISolutionLoader _solutionLoader;
    private readonly ILogger<SolutionFilterFileGenerationHandler> _logger;

    /// <inheritdoc cref="SolutionFilterFileGenerationHandler" />
    public SolutionFilterFileGenerationHandler(
        SolutionFilterGenerator solutionFilterGenerator,
        ISolutionFilterWriter solutionFilterWriter,
        ISolutionFiltersManifestLoader solutionFiltersConfigurationLoader,
        ISolutionLoader solutionLoader,
        ILogger<SolutionFilterFileGenerationHandler> logger
    )
    {
        _solutionFilterGenerator = solutionFilterGenerator;
        _solutionFilterWriter = solutionFilterWriter;
        _logger = logger;
        _solutionFiltersConfigurationLoader = solutionFiltersConfigurationLoader;
        _solutionLoader = solutionLoader;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    public void Handle(GenerateSolutionFiltersRequest request)
    {
        var filtersDefinition = _solutionFiltersConfigurationLoader.Load(request.FiltersConfigFilePath);
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
        _logger.LogInformation("Loaded solution file: {Path}", solutionFile.Path);

        _logger.LogInformation("Generating solution filters");
        var solutionFilters = _solutionFilterGenerator.GenerateMany(solutionFile, filtersDefinition).ToList();
        _logger.LogInformation("Generated {Count} solution filters", solutionFilters.Count);

        _solutionFilterWriter.WriteMany(solutionFilters, request.OutputDirectory);
        _logger.LogInformation("Solution filters written to: {OutputDirectory}", request.OutputDirectory);
    }
}
