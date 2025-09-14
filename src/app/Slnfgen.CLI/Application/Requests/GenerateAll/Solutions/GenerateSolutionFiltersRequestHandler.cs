using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Repository;
using Slnfgen.CLI.Domain.Solution.File.Repository;
using Slnfgen.CLI.Domain.Solution.File.Services;

namespace Slnfgen.CLI.Application.Requests.GenerateAll.Solutions;

/// <summary>
///
/// </summary>
public class GenerateSolutionsRequestHandler : IRequestHandler<GenerateSolutionsRequest, GenerateSolutionsResponse>
{
    private readonly ILogger<GenerateSolutionFiltersRequestHandler> _logger;
    private readonly SolutionGenerator _solutionGenerator;
    private readonly IXmlSolutionFileWriter _solutionWriter;
    private readonly ISolutionLoader _solutionLoader;
    private readonly ISolutionFiltersManifestLoader _solutionManifestLoader;

    /// <inheritdoc cref="GenerateSolutionFiltersRequestHandler" />
    public GenerateSolutionsRequestHandler(
        SolutionGenerator solutionGenerator,
        IXmlSolutionFileWriter solutionFileWriter,
        ISolutionFiltersManifestLoader solutionManifestLoader,
        ISolutionLoader solutionLoader,
        ILogger<GenerateSolutionFiltersRequestHandler> logger
    )
    {
        _solutionGenerator = solutionGenerator;
        _solutionWriter = solutionFileWriter;
        _logger = logger;
        _solutionManifestLoader = solutionManifestLoader;
        _solutionLoader = solutionLoader;
    }

    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    public GenerateSolutionsResponse Handle(GenerateSolutionsRequest request)
    {
        var manifest = _solutionManifestLoader.Load(request.ManifestFilePath);
        _logger.LogDebug(
            "Loaded filters file '{Path}' for Solution file '{SolutionFile}'",
            request.ManifestFilePath,
            manifest.SolutionFile
        );

        var solutionFilePath = Path.Combine(
            Directory.GetParent(request.ManifestFilePath)!.FullName,
            manifest.SolutionFile
        );
        var solutionFile = _solutionLoader.Load(solutionFilePath);
        _logger.LogDebug("Loaded solution file: {Path}", solutionFile.FileName);

        _logger.LogDebug("Generating solution filters");
        var solutionFiles = _solutionGenerator.GenerateMany(solutionFile, manifest).ToList();
        _logger.LogDebug("Generated {Count} solution filters", solutionFiles.Count);

        if (request.DryRun)
            return new GenerateSolutionsResponse(solutionFiles.Select(slnx => slnx.Name));

        var storedFilters = _solutionWriter.WriteMany(solutionFiles, request.OutputDirectory);
        _logger.LogDebug("Solution filters written to: {OutputDirectory}", request.OutputDirectory);
        return new GenerateSolutionsResponse(storedFilters);
    }
}
