using Cocona;
using Microsoft.Extensions.Logging;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.Solution;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;

namespace Slnfgen.CLI.Commands;

/// <summary>
///     
/// </summary>
public class GenerateSolutionFiltersCommand
{
    private readonly IRequestHandler<GenerateSolutionFiltersRequest> _handler;
    private readonly ILogger<GenerateSolutionFiltersCommand> _logger;

    public GenerateSolutionFiltersCommand(
        IRequestHandler<GenerateSolutionFiltersRequest> handler,
        ILogger<GenerateSolutionFiltersCommand> logger
    )
    {
        _handler = handler;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sln"></param>
    /// <param name="filtersFile"></param>
    [Command("gen")]
    public void Execute(
        [Option('s', Description = "Relative path to Solution file from which to generate filters")] string sln, 
        [Option('f', Description = "Relative path to Filters file which defines the desired filters")] string filtersFile,
        [Option('o', Description = "The output directory for the generated solution filters. Defaults to '.'")] string outDirectory = "."
    )
    {
        _handler.Handle(new GenerateSolutionFiltersRequest(sln, filtersFile, outDirectory));
    }

    
}