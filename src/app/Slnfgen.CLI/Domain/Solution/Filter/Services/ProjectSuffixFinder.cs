using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.Project.Models;

namespace Slnfgen.CLI.Domain.Solution.Filter.Services;

/// <summary>
///     Finds projects by suffix in a solution based on a set of patterns
/// </summary>
public class ProjectSuffixFinder
{
    /// <inheritdoc cref="ProjectSuffixFinder"/>
    public ProjectSuffixFinder() { }

    /// <summary>
    ///     Finds matching projects by suffix in a solution based on a set of patterns.
    ///     It searches through the projects in the solution and matches their names against the provided patterns
    ///     and returns the matching projects
    /// </summary>
    /// <param name="solutionFile">The parent solution file</param>
    /// <param name="projectsWithPotentialTestFiles"></param>
    /// <param name="autoIncludeSuffixes"></param>
    /// <returns></returns>
    public IEnumerable<SolutionProject> FindProjects(
        RootSolutionFile solutionFile,
        IEnumerable<SolutionProject> projectsWithPotentialTestFiles,
        ISet<string> autoIncludeSuffixes
    )
    {
        var testProjects = new HashSet<SolutionProject>();
        foreach (var project in projectsWithPotentialTestFiles)
        {
            var matchingProjects = solutionFile
                .ProjectsInSolution.Where(proj =>
                    proj.Name.StartsWith(
                        $"{project.Name}.", // to ensure we match projects like ProjA.UnitTests but not ProjAB.UnitTests
                        StringComparison.InvariantCultureIgnoreCase
                    )
                )
                .Where(proj =>
                    autoIncludeSuffixes.Any(pattern =>
                        proj.Name.Contains(pattern, StringComparison.InvariantCultureIgnoreCase)
                    )
                );

            testProjects.UnionWith(matchingProjects);
        }

        return testProjects;
    }
}
