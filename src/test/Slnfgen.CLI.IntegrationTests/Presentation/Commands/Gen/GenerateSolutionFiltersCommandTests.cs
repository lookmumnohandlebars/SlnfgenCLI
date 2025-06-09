using FluentAssertions;
using Slnfgen.CLI.IntegrationTests.Utilities.Fixtures;

namespace Slnfgen.CLI.IntegrationTests.Presentation.Commands;

public class GenerateSolutionFiltersCommandTests : IClassFixture<SolutionFilterFixture>
{
    private readonly CliRunner _cliRunner;
    private readonly SolutionFilterFixture _solutionFilterFixture;

    public GenerateSolutionFiltersCommandTests(SolutionFilterFixture fixture)
    {
        _cliRunner = new CliRunner();
        _solutionFilterFixture = fixture;
    }

    [Fact]
    public void GenCommand_ShouldGenerateMultipleSolutionFilterFiles()
    {
        var monrepoFilePath = Path.Combine(_solutionFilterFixture.DirectoryOfWork, "monorepo.yml");
        _cliRunner.Run("gen", "-f", monrepoFilePath, "-o", _solutionFilterFixture.DirectoryOfWork);
        var slnFilterOne = File.ReadAllText(Path.Combine(_solutionFilterFixture.DirectoryOfWork, "FilterOne.slnf"));
        slnFilterOne.Should().NotBeNullOrEmpty();
    }
}
