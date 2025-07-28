using Microsoft.Extensions.Logging;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;
using Slnfgen.CLI.Domain.Solution.File.Loader;

namespace Slnfgen.CLI.Application.Services.SolutionFilter;

/// <summary>
///     Generate a Solution Filter file
/// </summary>
// csharpier-ignore
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
    public GenerateSolutionFiltersResponse Handle(GenerateSolutionFiltersRequest request)
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

        _logger.LogInformation("Generating solution filters");
        var solutionFilters = _solutionFilterGenerator
            .GenerateMany(solutionFile, filtersDefinition, request.OutputDirectory)
            .ToList();
        _logger.LogInformation("Generated {Count} solution filters", solutionFilters.Count);

        if (request.DryRun)
            return new GenerateSolutionFiltersResponse(solutionFilters.Select(slnf => slnf.GetFileName()));
        var generatedFilters = _solutionFilterWriter.WriteMany(solutionFilters, request.OutputDirectory);
        _logger.LogInformation("Solution filters written to: {OutputDirectory}", request.OutputDirectory);
        return new GenerateSolutionFiltersResponse(generatedFilters);
    }
}
