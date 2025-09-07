using System.Text.Json;
using FluentAssertions;
using Slnfgen.CLI.Domain.Solution.Filter.Models;
using Slnfgen.CLI.IntegrationTests.SetUp.Fixtures;
using Slnfgen.CLI.IntegrationTests.SetUp.Utilities;

namespace Slnfgen.CLI.IntegrationTests.Commands.Gen;

[Collection(nameof(GenerateSolutionFiltersTestCollection))]
public class GenerateAllSolutionFiltersCommandTests : IClassFixture<SolutionFilterFixture>
{
    private readonly CliRunner _cliRunner;
    private readonly SolutionFilterFixture _solutionFilterFixture;

    public GenerateAllSolutionFiltersCommandTests(SolutionFilterFixture fixture)
    {
        _cliRunner = new CliRunner();
        _solutionFilterFixture = fixture;
    }

    private string ExecuteCommand(string outputDirectory = "")
    {
        var monrepoFilePath = Path.Combine(_solutionFilterFixture.DirectoryOfWork, "monorepo.yml");
        return _cliRunner.Run(
            "all",
            monrepoFilePath,
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
                    @"Project1.Unit.Tests\\Project1.Unit.Tests.csproj",
                    @"Project2\\Project2.csproj",
                    @"Project2.Unit.Tests\\Project2.Unit.Tests.csproj",
                    @"Project3\\Project3.csproj",
                    @"Project3.Unit.Tests\\Project3.Unit.Tests.csproj",
                    @"Project4\\Project4.csproj",
                    @"Project4.Unit.Tests\\Project4.Unit.Tests.csproj",
                    @"Project5\\Project5.csproj",
                    @"Project5.Unit.Tests\\Project5.Unit.Tests.csproj",
                    @"Project6\\Project6.csproj",
                    @"Project6.Unit.Tests\\Project6.Unit.Tests.csproj",
                ],
                "Solution filter should contain the correct projects"
            );

        var slnFilterTwo = LoadSolutionFilter(Path.Combine(_solutionFilterFixture.DirectoryOfWork, "FilterTwo.slnf"));
        slnFilterTwo.Solution.Path.Should().Be("TestSolution.sln");
        slnFilterTwo
            .Solution.Projects.Should()
            .BeEquivalentTo(
                [
                    @"Project7\\Project7.csproj",
                    @"Project7.Unit.Tests\\Project7.Unit.Tests.csproj",
                    @"Project8\\Project8.csproj",
                    @"Project8.Unit.Tests\\Project8.Unit.Tests.csproj",
                    @"Project9\\Project9.csproj",
                    @"Project9.Unit.Tests\\Project9.Unit.Tests.csproj",
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
                    @"Project1.Unit.Tests\\Project1.Unit.Tests.csproj",
                    @"Project2\\Project2.csproj",
                    @"Project2.Unit.Tests\\Project2.Unit.Tests.csproj",
                    @"Project3\\Project3.csproj",
                    @"Project3.Unit.Tests\\Project3.Unit.Tests.csproj",
                    @"Project4\\Project4.csproj",
                    @"Project4.Unit.Tests\\Project4.Unit.Tests.csproj",
                    @"Project5\\Project5.csproj",
                    @"Project5.Unit.Tests\\Project5.Unit.Tests.csproj",
                    @"Project6\\Project6.csproj",
                    @"Project6.Unit.Tests\\Project6.Unit.Tests.csproj",
                ],
                "Solution filter should contain the correct projects"
            );

        var slnFilterTwo = LoadSolutionFilter(
            Path.Combine(_solutionFilterFixture.DirectoryOfWork, "SubService", "FilterTwo.slnf")
        );
        slnFilterTwo.Solution.Path.Should().Be("..\\\\TestSolution.sln");
        slnFilterTwo
            .Solution.Projects.Should()
            .BeEquivalentTo(
                [
                    "Project7\\\\Project7.csproj",
                    "Project7.Unit.Tests\\\\Project7.Unit.Tests.csproj",
                    "Project8\\\\Project8.csproj",
                    "Project8.Unit.Tests\\\\Project8.Unit.Tests.csproj",
                    "Project9\\\\Project9.csproj",
                    "Project9.Unit.Tests\\\\Project9.Unit.Tests.csproj",
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
