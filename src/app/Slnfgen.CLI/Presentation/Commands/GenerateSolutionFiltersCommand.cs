using Cocona;
using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;
using Spectre.Console;

namespace Slnfgen.CLI.Presentation.Commands;

/// <summary>
///     The command to generate solution filters from a filters manifest file
/// </summary>
public class GenerateSolutionFiltersCommand
{
    private readonly IRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse> _handler;

    private readonly ILogger<GenerateSolutionFiltersCommand> _logger;

    /// <inheritdoc cref="GenerateSolutionFiltersCommand" />
    /// <param name="handler">The request Handler</param>
    /// <param name="logger">Logger for </param>
    public GenerateSolutionFiltersCommand(
        IRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse> handler,
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
    /// <param name="dryRun"></param>
    [Command("gen", Description = "Generates .NET solution filters (.slnf) based on the provided manifest file")]
    public void Execute(
        [Option('f', Description = "Relative path to Filters file which defines the desired filters")]
            string filtersFile,
        [Option('o', Description = "The output directory for the generated solution filters. Defaults to '.'")]
            string outDirectory = ".",
        [Option("dryrun", Description = "Enable verbose output")] bool dryRun = false
    )
    {
        var res = AnsiConsole
            .Status()
            .Start(
                "Generating solution filters...",
                ctx =>
                {
                    ctx.Spinner(Spinner.Known.Dots8Bit);
                    ctx.SpinnerStyle(Style.Parse("blue"));

                    _logger.LogDebug(
                        "Generating solution filters from {FiltersFile} to {OutDirectory}",
                        filtersFile,
                        outDirectory
                    );
                    return _handler.Handle(new GenerateSolutionFiltersRequest(filtersFile, outDirectory, dryRun));
                }
            );
        AnsiConsole.MarkupLine("[green]Solution filters generated successfully![/]");
    }
}
