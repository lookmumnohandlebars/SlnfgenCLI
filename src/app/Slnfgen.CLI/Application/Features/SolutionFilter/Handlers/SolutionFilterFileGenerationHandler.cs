using Microsoft.Extensions.Logging;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.Solution;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;

namespace Slnfgen.CLI.Application.Services.SolutionFilter;

public class SolutionFilterFileGenerationHandler : IRequestHandler<GenerateSolutionFiltersRequest>
{
    private readonly SolutionFilterGenerator _solutionFilterGenerator;
    private readonly SolutionFilterWriter _solutionFilterWriter;
    private readonly ILogger<SolutionFilterFileGenerationHandler> _logger;

    public SolutionFilterFileGenerationHandler(
        SolutionFilterGenerator solutionFilterGenerator,
        SolutionFilterWriter solutionFilterWriter, 
        ILogger<SolutionFilterFileGenerationHandler> logger
    )
    {
        _solutionFilterGenerator = solutionFilterGenerator;
        _solutionFilterWriter = solutionFilterWriter;
        _logger = logger;
    }

    public void Handle(GenerateSolutionFiltersRequest request)
    {
        var solutionFile = RootSolutionFile.FromSolutionFilePath(request.SolutionFilePath);
        _logger.LogInformation("Loaded solution file: {Path}", solutionFile.Path);
        
        var filtersDefinition = SolutionFiltersConfiguration.FromFile(request.FiltersConfigFilePath);
        _logger.LogInformation("Loaded filters file '{Path}' for Solution file '{SolutionFile}'", request.FiltersConfigFilePath, filtersDefinition.SolutionFile);
        
        _logger.LogInformation("Generating solution filters");
        var solutionFilters = _solutionFilterGenerator.GenerateMany(solutionFile, filtersDefinition).ToList();
        _logger.LogInformation("Generated {Count} solution filters", solutionFilters.Count);
        
        _solutionFilterWriter.WriteMany(solutionFilters, request.OutputDirectory);
        _logger.LogInformation("Solution filters written to: {OutputDirectory}", request.OutputDirectory);
    }
}