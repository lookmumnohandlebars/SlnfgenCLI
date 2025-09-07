using Microsoft.Build.Construction;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.Project.Models;
using Slnfgen.CLI.Domain.Solution.Project.Repositories;

namespace Slnfgen.CLI.Domain.Solution.Filter.Services;

/// <summary>
///     Finds test projects in a solution based on a set of patterns
/// </summary>
public class TestProjectFinder
{
    /// <inheritdoc cref="TestProjectFinder"/>
    public TestProjectFinder() { }

    /// <summary>
    ///     Finds test projects in a solution file based on a set of patterns.
    ///     It searches through the projects in the solution and matches their names against the provided patterns
    ///     and returns the matching
    /// </summary>
    /// <param name="solutionFile">The parent solution file</param>
    /// <param name="projectsWithPotentialTestFiles"></param>
    /// <param name="testProjectPatterns"></param>
    /// <returns></returns>
    public IEnumerable<SolutionProject> FindTestProjects(
        RootSolutionFile solutionFile,
        IEnumerable<SolutionProject> projectsWithPotentialTestFiles,
        ISet<string> testProjectPatterns
    )
    {
        var testProjects = new HashSet<SolutionProject>();
        foreach (var project in projectsWithPotentialTestFiles)
        {
            var matchingTestProjects = solutionFile
                .ProjectsInSolution.Where(proj =>
                    proj.Name.Contains(
                        $"{project.Name}.", // to ensure we match projects like ProjA.UnitTests but not ProjAB.UnitTests
                        StringComparison.InvariantCultureIgnoreCase
                    )
                )
                .Where(proj =>
                    testProjectPatterns.Any(pattern =>
                        proj.Name.Contains(pattern, StringComparison.InvariantCultureIgnoreCase)
                    )
                );

            testProjects.UnionWith(matchingTestProjects);
        }

        return testProjects;
    }
}
