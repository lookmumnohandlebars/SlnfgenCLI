using Cocona;
using Slnfgen.CLI.Application.Common.Requests;
using Slnfgen.CLI.Application.Common.Requests.Validation;
using Slnfgen.CLI.Application.Requests.GenerateTarget;
using Spectre.Console;

namespace Slnfgen.CLI.Presentation.Commands;

/// <summary>
///     Generates a single solution filter based on the provided filters file.
///     This command is used to generate a specific solution filter defined in the manifest file.
///     It allows users to specify the target filter name and the output directory for the generated filter.
///     If the target filter name is not provided, an exception will be thrown.
///     The command supports a dry run mode, which enables verbose output without writing any files.
/// </summary>
public class GenerateOneSolutionFilterCommand
{
    private readonly IRequestHandler<
        GenerateTargetSolutionFilterRequest,
        GenerateTargetSolutionFilterResponse
    > _handler;

    /// <inheritdoc cref="GenerateAllSolutionFiltersCommand" />
    /// <param name="handler">The request Handler</param>
    public GenerateOneSolutionFilterCommand(
        IRequestHandler<GenerateTargetSolutionFilterRequest, GenerateTargetSolutionFilterResponse> handler
    )
    {
        _handler = handler;
    }

    /// <summary>
    ///     Generates solution filters based on the provided filters file
    /// </summary>
    /// <param name="filtersFile"></param>
    /// <param name="targetFilterName"></param>
    /// <param name="outDirectory"></param>
    /// <param name="dryRun"></param>
    [Command("target", Description = "Generates .NET solution filters (.slnf) based on the provided manifest file")]
    public void Execute(
        [Argument("manifest file", Description = "Relative path to Filters file which defines the desired filters")]
            string filtersFile,
        [Option(
            "target",
            ['t'],
            Description = "The name of the target filter to generate. This should match a filter defined in the manifest file."
        )]
            string targetFilterName,
        [Option(
            "output",
            ['o'],
            Description = "The output directory for the generated solution filters. Defaults to '.'"
        )]
            string outDirectory = ".",
        [Option("dryrun", Description = "Enable verbose output")] bool dryRun = false
    )
    {
        if (string.IsNullOrEmpty(targetFilterName))
            throw new BadRequestException($"{nameof(targetFilterName)} is required.");

        AnsiConsole
            .Status()
            .Start(
                "Generating solution filter...",
                ctx =>
                {
                    ctx.Spinner(Spinner.Known.Dots8Bit);
                    ctx.SpinnerStyle(Style.Parse("blue"));

                    return _handler.Handle(
                        new GenerateTargetSolutionFilterRequest(filtersFile, targetFilterName, outDirectory, dryRun)
                    );
                }
            );
        AnsiConsole.MarkupLine("[green]Solution filter generated successfully![/]");
    }
}
