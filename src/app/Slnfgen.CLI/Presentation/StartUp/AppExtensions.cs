using Cocona;
using Cocona.Builder;
using Slnfgen.CLI.Commands;
using Slnfgen.CLI.Presentation.Filters;

namespace Slnfgen.CLI.Presentation.StartUp;

internal static class AppExtensions
{
    public static void AddAllMiddleware(this ICoconaCommandsBuilder app)
    {
        app.UseFilter(new DebugFilter());
    }

    public static void AddAllCommands(this ICoconaCommandsBuilder app)
    {
        app.AddCommands<GenerateSolutionFiltersCommand>();
    }
}
