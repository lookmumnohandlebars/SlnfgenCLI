using System.Reflection.Emit;
using Cocona.Command;
using Cocona.CommandLine;
using Cocona.Filters;

namespace Slnfgen.CLI.UnitTests.Presentation.Filters;

public class FilterTestsBase
{
    protected CommandExecutionDelegate EmptyCommandDelegate = _ => new ValueTask<int>(3);

    protected CoconaCommandExecutingContext EmptyCommandContext =>
        new(
            new CommandDescriptor(
                new DynamicMethod(string.Empty, typeof(int), new Type[] { }),
                "TestCommand",
                "TestCommand",
                new List<string>(),
                "TestDescription",
                new List<object>(),
                new List<ICommandParameterDescriptor>(),
                new List<CommandOptionDescriptor>(),
                new List<CommandArgumentDescriptor>(),
                new List<CommandOverloadDescriptor>(),
                new List<CommandOptionLikeCommandDescriptor>(),
                CommandFlags.None,
                null
            ),
            ParsedCommandLine.Empty,
            null
        );
}
