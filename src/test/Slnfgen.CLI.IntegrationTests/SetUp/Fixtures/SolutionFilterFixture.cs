using Slnfgen.Application.Domain.Filters;

namespace Slnfgen.CLI.IntegrationTests.Utilities.Fixtures;

/// <summary>
///     Fixture for setting up a test solution with multiple projects and dependencies required for solution filtering
///     tests
/// </summary>
public class SolutionFilterFixture : IDisposable
{
    private readonly DotnetCommandRunner _dotnetRunner;

    public SolutionFilterFixture()
    {
        var tmpDir = Path.Combine(Path.GetTempPath(), "Slnfgen.CLI.IntegrationTests", Guid.NewGuid().ToString());
        DirectoryOfWork = Directory.CreateDirectory(tmpDir).FullName;
        _dotnetRunner = new DotnetCommandRunner(DirectoryOfWork);

        var projectNames = CreateTestSolutionWithProjects().ToList();
        AddProjectDependencies(projectNames);
        WriteManifestFile(DirectoryOfWork);
    }

    public string DirectoryOfWork { get; }

    /// <summary>
    ///     Cleans up the fixture by deleting the temporary directory and its contents.
    /// </summary>
    public void Dispose()
    {
        try
        {
            if (Directory.Exists(DirectoryOfWork))
                // Delete all files and subdirectories
                Directory.Delete(Path.Combine(DirectoryOfWork, ".."), true);
        }
        catch (Exception ex)
        {
            // Log or handle any cleanup errors
            Console.WriteLine($"Error during cleanup: {ex.Message}");
        }
    }

    private void WriteManifestFile(string directoryOfWork)
    {
        var manifestWriter = new ConfigurationFileWriter(directoryOfWork);
        var filters = new SolutionFiltersManifest(
            "TestSolution.sln",
            [
                new SolutionFiltersManifestFilterDefinition(
                    "FilterOne",
                    ["Project1/Project1.csproj", "Project4/Project4.csproj"]
                ),
                new SolutionFiltersManifestFilterDefinition("FilterTwo", ["Project7/Project7.csproj"]),
            ]
        );
        manifestWriter.Write("monorepo.yml", filters);
    }

    private void AddProjectDependencies(IReadOnlyList<string> projectNames)
    {
        for (var i = 0; i < projectNames.Count; i++)
        {
            // Only process every third project (0, 3, 6, 9 in zero-based index)
            if (i % 3 != 0)
                continue;

            var projectPath = Path.Combine(projectNames[i], $"{projectNames[i]}.csproj");

            // Add reference to next two projects if they exist
            for (var j = 1; j <= 2; j++)
            {
                var dependencyIndex = i + j;
                if (dependencyIndex < projectNames.Count)
                {
                    var dependencyPath = Path.Combine(
                        projectNames[dependencyIndex],
                        $"{projectNames[dependencyIndex]}.csproj"
                    );
                    _dotnetRunner.AddProjectReference(projectPath, dependencyPath);
                }
            }
        }
    }

    private IEnumerable<string> CreateTestSolutionWithProjects()
    {
        // Create a new solution
        _dotnetRunner.CreateNewSolution("TestSolution");

        // Create 10 projects and add them to the solution
        for (var i = 1; i <= 10; i++)
        {
            var projectName = $"Project{i}";
            _dotnetRunner.CreateProjectAndAddToSolution(projectName);
            yield return projectName;
        }
    }
}
