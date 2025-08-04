using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Domain.Project;
using Slnfgen.Application.Features.Solution;
using Slnfgen.Application.Features.SolutionFilter;
using Slnfgen.CLI;
using Slnfgen.CLI.Domain.Solution.Project.Repositories;

namespace Slnfgen.Application.Features.SolutionFilterGeneration;

/// <summary>
/// </summary>
public class SolutionFilterGenerator
{
    private readonly IProjectFileLoader _projectFileLoader;

    /// <inheritdoc cref="SolutionFilterGenerator"/>
    public SolutionFilterGenerator(IProjectFileLoader projectFileLoader)
    {
        _projectFileLoader = projectFileLoader;
    }

    /// <summary>
    /// </summary>
    /// <param name="solutionFile"></param>
    /// <param name="filters"></param>
    /// <param name="targetDirectory"></param>
    /// <returns></returns>
    public IEnumerable<SolutionFilter.SolutionFilter> GenerateMany(
        RootSolutionFile solutionFile,
        SolutionFiltersManifest filters,
        string targetDirectory
    ) =>
        filters.FilterDefinitions.Select(filterDefinition =>
            GenerateFromFilterDefinition(solutionFile, filterDefinition, targetDirectory)
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
    public SolutionFilter.SolutionFilter GenerateForTarget(
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

        return GenerateFromFilterDefinition(solutionFile, targetFilter, targetDirectory);
    }

    #region Internal

    private SolutionFilter.SolutionFilter GenerateFromFilterDefinition(
        RootSolutionFile solutionFile,
        SolutionFiltersManifestFilterDefinition filterDefinition,
        string targetDirectory
    )
    {
        var entryPointProjects = GetEntryPointSolutionProjects(solutionFile, filterDefinition).ToList();

        var entryPointProjectsWithDependencies = AggregateProjectDependencies(entryPointProjects, solutionFile);

        var relativePathToSolutionFile = Path.GetRelativePath(
            Path.Combine(Directory.GetCurrentDirectory(), targetDirectory),
            solutionFile.AbsolutePath
        );

        return new SolutionFilter.SolutionFilter(
            filterDefinition.Name,
            new SolutionFiltersManifestSolutionDefinition(
                relativePathToSolutionFile,
                entryPointProjectsWithDependencies.ToArray()
            )
        );
    }

    private IEnumerable<SolutionProject> GetEntryPointSolutionProjects(
        RootSolutionFile solutionFile,
        SolutionFiltersManifestFilterDefinition filterDefinition
    ) =>
        solutionFile.ProjectsInSolution.Where(proj =>
            filterDefinition.Entrypoints.Contains(proj.Name) || filterDefinition.Entrypoints.Contains(proj.Path)
        );

    private IEnumerable<string> AggregateProjectDependencies(
        IEnumerable<SolutionProject> projects,
        RootSolutionFile solutionFile
    )
    {
        var stack = new Stack<SolutionProject>(projects);
        var includedProjectPaths = new HashSet<string>();
        while (stack.TryPop(out var currentProject))
        {
            includedProjectPaths.Add(currentProject.Path);
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
                    !includedProjectPaths.Contains(NormalizePath(proj.Path))
                )
            )
            {
                stack.Push(dependantProjectInSolution);
            }
        }

        return includedProjectPaths.OrderBy(path => path);
    }

    private static string NormalizePath(string path)
    {
        return path.Replace("..\\", string.Empty).Replace("/", "\\"); //TODO: this is a temporary fix, should be handled by the loader
    }

    #endregion
}
