using Cocona;
using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Spectre.Console;

namespace Slnfgen.CLI.Presentation.Commands;

/// <summary>
///     The command to generate solution filters from a filters manifest file.
///     All solution filters defined in the manifest will be generated
/// </summary>
public class GenerateAllSolutionFiltersCommand
{
    private readonly IRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse> _handler;

    private readonly ILogger<GenerateAllSolutionFiltersCommand> _logger;

    /// <inheritdoc cref="GenerateAllSolutionFiltersCommand" />
    /// <param name="handler">The request handler</param>
    /// <param name="logger">Logger</param>
    public GenerateAllSolutionFiltersCommand(
        IRequestHandler<GenerateSolutionFiltersRequest, GenerateSolutionFiltersResponse> handler,
        ILogger<GenerateAllSolutionFiltersCommand> logger
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
    [Command("all", Description = "Generates .NET solution filters (.slnf) based on the provided manifest file")]
    public void Execute(
        [Argument("manifest file", Description = "Relative path to Filters file which defines the desired filters")]
            string filtersFile,
        [Option(
            "output",
            ['o'],
            Description = "The output directory for the generated solution filters. Defaults to '.'"
        )]
            string outDirectory = ".",
        [Option("dryrun", Description = "Enable verbose output")] bool dryRun = false
    )
    {
        AnsiConsole
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
