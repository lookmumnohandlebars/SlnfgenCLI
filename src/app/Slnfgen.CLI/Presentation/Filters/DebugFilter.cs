using System.Diagnostics;
using Cocona.Filters;
using Microsoft.Extensions.Logging;

namespace Slnfgen.CLI.Presentation.Filters;

/// <summary>
///     A command filter that launches the debugger when a command is executed.
/// </summary>
internal class DebugFilter : CommandFilterAttribute
{
    private readonly ILogger _logger;

    public DebugFilter(ILogger logger)
    {
        _logger = logger;
    }

    public override async ValueTask<int> OnCommandExecutionAsync(
        CoconaCommandExecutingContext ctx,
        CommandExecutionDelegate next
    )
    {
        _logger.LogDebug($"Running Command in debug: {ctx.Command.Name}");
        var hasLaunched = Debugger.Launch();
        var attemptCount = 0;
        while (!hasLaunched && !Debugger.IsAttached && attemptCount < 5)
        {
            attemptCount++;
            _logger.LogDebug($"Debugger not attached, retrying {attemptCount}/5...");
            Thread.Sleep(500); // Allow time for the debugger to attach
            hasLaunched = Debugger.Launch();
        }

        return await next(ctx);
    }
}
