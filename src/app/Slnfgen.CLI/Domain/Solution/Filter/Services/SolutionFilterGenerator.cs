using Slnfgen.CLI.Application.Common.Requests.Validation;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.Filter.Models;
using Slnfgen.CLI.Domain.Solution.Project.Models;
using Slnfgen.CLI.Domain.Solution.Project.Repositories;

namespace Slnfgen.CLI.Domain.Solution.Filter.Services;

/// <summary>
///
/// </summary>
public class SolutionFilterGenerator
{
    private readonly IProjectFileLoader _projectFileLoader;
    private readonly ProjectSuffixFinder _projectSuffixFinder;

    /// <inheritdoc cref="SolutionFilterGenerator"/>
    public SolutionFilterGenerator(IProjectFileLoader projectFileLoader, ProjectSuffixFinder projectSuffixFinder)
    {
        _projectFileLoader = projectFileLoader;
        _projectSuffixFinder = projectSuffixFinder;
    }

    /// <summary>
    /// </summary>
    /// <param name="solutionFile"></param>
    /// <param name="filters"></param>
    /// <param name="targetDirectory"></param>
    /// <returns></returns>
    public IEnumerable<SolutionFilter> GenerateMany(
        RootSolutionFile solutionFile,
        SolutionFiltersManifest filters,
        string targetDirectory
    ) =>
        filters.FilterDefinitions.Select(filterDefinition =>
            GenerateFromFilterDefinition(solutionFile, filters, filterDefinition, targetDirectory)
        );

    /// <summary>
    ///     Generates a solution filter for a specific target defined in the manifest file.
    /// </summary>
    /// <param name="targetSolutionFilterName"></param>
    /// <param name="solutionFile"></param>
    /// <param name="filters"></param>
    /// <param name="targetDirectory"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public SolutionFilter GenerateForTarget(
        string targetSolutionFilterName,
        RootSolutionFile solutionFile,
        SolutionFiltersManifest filters,
        string targetDirectory
    )
    {
        var targetFilter = filters.FilterDefinitions.SingleOrDefault(filterDefinition =>
            filterDefinition.Name.Equals(targetSolutionFilterName, StringComparison.OrdinalIgnoreCase)
        );
        if (targetFilter == null)
        {
            throw new BadRequestException("Filter definition in manifest file: " + targetSolutionFilterName);
        }

        return GenerateFromFilterDefinition(solutionFile, filters, targetFilter, targetDirectory);
    }

    #region Internal

    private SolutionFilter GenerateFromFilterDefinition(
        RootSolutionFile solutionFile,
        SolutionFiltersManifest manifest,
        SolutionFiltersManifestFilterDefinition filterDefinition,
        string targetDirectory
    )
    {
        var entryPointProjectsWithDependencies = GetDependantProjectsFromEntryPoints(solutionFile, filterDefinition);
        var testProjectsWithDependencies = GetDependantProjectsFromTests(
            solutionFile,
            manifest,
            filterDefinition,
            entryPointProjectsWithDependencies
        );

        var allIncludedProjectFileNames = entryPointProjectsWithDependencies
            .Union(testProjectsWithDependencies)
            .Select(proj => proj.Path)
            .Distinct()
            .ToList();

        var relativePathToSolutionFile = Path.GetRelativePath(
            Path.Combine(Directory.GetCurrentDirectory(), targetDirectory),
            solutionFile.AbsolutePath
        );

        return new SolutionFilter(
            filterDefinition.Name,
            new SolutionFiltersSolutionDefinition(
                relativePathToSolutionFile,
                allIncludedProjectFileNames.OrderBy(path => path).ToArray()
            )
        );
    }

    private ISet<SolutionProject> GetDependantProjectsFromEntryPoints(
        RootSolutionFile solutionFile,
        SolutionFiltersManifestFilterDefinition filterDefinition
    ) =>
        AggregateProjectDependencies(
            GetEntryPointSolutionProjects(solutionFile, filterDefinition).ToList(),
            solutionFile
        );

    private ISet<SolutionProject> GetDependantProjectsFromTests(
        RootSolutionFile solutionFile,
        SolutionFiltersManifest manifest,
        SolutionFiltersManifestFilterDefinition filterDefinition,
        IEnumerable<SolutionProject> entryPointProjectsWithDependencies
    )
    {
        var autoIncludedTestProjects = GetTestProjects(
            solutionFile,
            manifest,
            filterDefinition,
            entryPointProjectsWithDependencies
        );

        return AggregateProjectDependencies(autoIncludedTestProjects, solutionFile);
    }

    private IEnumerable<SolutionProject> GetEntryPointSolutionProjects(
        RootSolutionFile solutionFile,
        SolutionFiltersManifestFilterDefinition filterDefinition
    ) =>
        solutionFile.ProjectsInSolution.Where(proj =>
            filterDefinition.Entrypoints.Contains(proj.Name) || filterDefinition.Entrypoints.Contains(proj.Path)
        );

    private ISet<SolutionProject> AggregateProjectDependencies(
        IEnumerable<SolutionProject> projects,
        RootSolutionFile solutionFile
    )
    {
        var stack = new Stack<SolutionProject>(projects);
        var includedProjects = new HashSet<SolutionProject>();
        while (stack.TryPop(out var currentProject))
        {
            includedProjects.Add(currentProject);
            var dependenciesInProjectFile = _projectFileLoader
                .LoadOne(Path.Combine(solutionFile.AbsolutePath, "..", currentProject.Path))
                .ProjectDependencies.Select(NormalizePath)
                .ToList();
            var dependantProjectsInSolution = solutionFile
                .ProjectsInSolution.Where(proj =>
                    dependenciesInProjectFile.Contains(NormalizePath(proj.Path))
                    || dependenciesInProjectFile.Contains(NormalizePath(proj.Name))
                )
                .ToList();
            foreach (
                var dependantProjectInSolution in dependantProjectsInSolution.Where(proj =>
                    !includedProjects.Select(includedProject => includedProject.Path).Contains(NormalizePath(proj.Path))
                )
            )
            {
                stack.Push(dependantProjectInSolution);
            }
        }

        return includedProjects;
    }

    private ISet<SolutionProject> GetTestProjects(
        RootSolutionFile solutionFile,
        SolutionFiltersManifest manifest,
        SolutionFiltersManifestFilterDefinition manifestFilterDefinition,
        IEnumerable<SolutionProject> projects
    )
    {
        var combinedTestProjectPatterns = manifestFilterDefinition
            .AutoIncludeSuffixPatterns.ToHashSet()
            .Union(manifest.AutoIncludeSuffixPatterns.ToHashSet())
            .ToHashSet();
        return _projectSuffixFinder.FindProjects(solutionFile, projects, combinedTestProjectPatterns).ToHashSet();
    }

    private static string NormalizePath(string path)
    {
        return path.Replace("..\\", string.Empty).Replace("/", "\\"); //TODO: this is a temporary fix, should be handled by the loader
    }

    #endregion
}
