using System.Text.Json;
using FluentAssertions;
using Slnfgen.CLI.Domain.Solution.Filter.Models;
using Slnfgen.CLI.IntegrationTests.SetUp.Fixtures;
using Slnfgen.CLI.IntegrationTests.SetUp.Utilities;

namespace Slnfgen.CLI.IntegrationTests.Commands.Gen;

[Collection(nameof(GenerateSolutionFiltersTestCollection))]
public class GenerateTargetSolutionFiltersCommandTests : IClassFixture<SolutionFilterFixture>
{
    private readonly CliRunner _cliRunner;
    private readonly SolutionFilterFixture _solutionFilterFixture;

    public GenerateTargetSolutionFiltersCommandTests(SolutionFilterFixture fixture)
    {
        _cliRunner = new CliRunner();
        _solutionFilterFixture = fixture;
    }

    private string ExecuteCommand(string outputDirectory = "", string target = "FilterOne")
    {
        var monrepoFilePath = Path.Combine(_solutionFilterFixture.DirectoryOfWork, "monorepo.yml");
        return _cliRunner.Run(
            "target",
            monrepoFilePath,
            "-t",
            target,
            "-o",
            Path.Combine(_solutionFilterFixture.DirectoryOfWork, outputDirectory)
        );
    }

    [Fact]
    public void GenCommand_ShouldGenerateMultipleSolutionFilterFiles()
    {
        ExecuteCommand();
        var slnFilterOne = LoadSolutionFilter(Path.Combine(_solutionFilterFixture.DirectoryOfWork, "FilterOne.slnf"));

        slnFilterOne.Solution.Path.Should().Be("TestSolution.sln");
        slnFilterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                [
                    @"Project1\\Project1.csproj",
                    @"Project2\\Project2.csproj",
                    @"Project3\\Project3.csproj",
                    @"Project4\\Project4.csproj",
                    @"Project5\\Project5.csproj",
                    @"Project6\\Project6.csproj",
                ],
                "Solution filter should contain the correct projects"
            );
    }

    [Fact]
    public void GenCommand_ShouldGenerateMultipleSolutionFilterFilesWithinSubdirectory()
    {
        ExecuteCommand("SubService");
        var slnFilterOne = LoadSolutionFilter(
            Path.Combine(_solutionFilterFixture.DirectoryOfWork, "SubService", "FilterOne.slnf")
        );

        slnFilterOne.Solution.Path.Should().Be(@"..\\TestSolution.sln");
        slnFilterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                [
                    @"Project1\\Project1.csproj", // because it's relative to the solution
                    @"Project2\\Project2.csproj",
                    @"Project3\\Project3.csproj",
                    @"Project4\\Project4.csproj",
                    @"Project5\\Project5.csproj",
                    @"Project6\\Project6.csproj",
                ],
                "Solution filter should contain the correct projects"
            );
    }

    #region Helpers

    private static SolutionFilter LoadSolutionFilter(string solutionFilePath)
    {
        var slnFilterAsText = File.ReadAllText(solutionFilePath);
        slnFilterAsText.Should().NotBeNullOrEmpty("Solution filter file should not be empty");

        return JsonSerializer.Deserialize<SolutionFilter>(slnFilterAsText)
            ?? throw new InvalidOperationException("Failed to deserialize solution filter");
    }

    #endregion
}
