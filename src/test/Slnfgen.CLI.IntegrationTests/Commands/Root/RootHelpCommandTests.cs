using FluentAssertions;
using Slnfgen.CLI.IntegrationTests.SetUp.Utilities;

namespace Slnfgen.CLI.IntegrationTests.Commands.Root;

public class RootHelpCommandTests
{
    private readonly CliRunner _cliRunner;

    public RootHelpCommandTests()
    {
        _cliRunner = new CliRunner(
            Directory.GetCurrentDirectory(),
            Path.Combine("..", "..", "..", "..", "..", "app", "Slnfgen.CLI", "Slnfgen.CLI.csproj")
        );
    }

    [Fact]
    public void HelpCommand_ShouldDisplayRootHelp()
    {
        var result = _cliRunner.Run("--help");
        result.Should().Contain("Usage: Slnfgen.CLI");
    }
}
