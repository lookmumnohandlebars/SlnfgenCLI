using System.Diagnostics;
using Cocona;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;

namespace Slnfgen.CLI.Commands;

/// <summary>
///     The command class to generate solution filters from a filters manifest file
/// </summary>
public class GenerateSolutionFiltersCommand
{
    private readonly IRequestHandler<GenerateSolutionFiltersRequest> _handler;
    private readonly ILogger<GenerateSolutionFiltersCommand> _logger;

    /// <summary>
    ///     Cre
    /// </summary>
    /// <param name="handler">The request Handler</param>
    /// <param name="logger"></param>
    public GenerateSolutionFiltersCommand(
        IRequestHandler<GenerateSolutionFiltersRequest> handler,
        ILogger<GenerateSolutionFiltersCommand> logger
    )
    {
        _handler = handler;
        _logger = logger;
    }

    /// <summary>
    ///     Generates solution filters based on the provided filters file
    /// </summary>
    /// <param name="filtersFile"></param>
    /// <param name="outDirectory"></param>
    [Command("gen", Description = "Generates .NET solution filters (.slnf) based on the provided manifest file")]
    [UsedImplicitly]
    public void Execute(
        [Option('f', Description = "Relative path to Filters file which defines the desired filters")]
            string filtersFile,
        [Option('o', Description = "The output directory for the generated solution filters. Defaults to '.'")]
            string outDirectory = "."
    )
    {
        _handler.Handle(new GenerateSolutionFiltersRequest(filtersFile, outDirectory));
    }
}
