using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Presentation.Commands;
using Slnfgen.CLI.Presentation.Filters;

namespace Slnfgen.CLI.Presentation.StartUp;

internal static class AppExtensions
{
    public static void AddAllMiddleware(this ICoconaCommandsBuilder app, ILogger logger)
    {
        app.UseFilter(new DebugFilter(logger));
        app.UseFilter(new ExceptionFilter(logger));
    }

    public static void AddAllCommands(this ICoconaCommandsBuilder app)
    {
        app.AddCommands<GenerateAllSolutionFiltersCommand>();
        app.AddCommands<GenerateOneSolutionFilterCommand>();
    }
}
