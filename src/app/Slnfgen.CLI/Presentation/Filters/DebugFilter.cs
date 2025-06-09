using System.Diagnostics;
using Cocona.Filters;

namespace Slnfgen.CLI.Presentation.Filters;

internal class DebugFilter : CommandFilterAttribute
{
    public override async ValueTask<int> OnCommandExecutionAsync(
        CoconaCommandExecutingContext ctx,
        CommandExecutionDelegate next
    )
    {
        Console.WriteLine($"Running Command in debug: {ctx.Command.Name}");
        Debugger.Launch();
        return await next(ctx);
    }
}
