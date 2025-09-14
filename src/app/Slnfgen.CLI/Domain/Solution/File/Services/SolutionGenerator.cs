using Slnfgen.CLI.Application.Common.Requests.Validation;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.Filter.Services;
using Slnfgen.CLI.Domain.Solution.Project.Models;
using Slnfgen.CLI.Domain.Solution.Project.Repositories;
using Slnfgen.CLI.Domain.Solution.Project.Services;

namespace Slnfgen.CLI.Domain.Solution.File.Services;

/// <summary>
///
/// </summary>
public class SolutionGenerator
{
    private readonly IProjectFileLoader _projectFileLoader;
    private readonly ProjectSuffixFinder _projectSuffixFinder;

    /// <inheritdoc cref="SolutionFilterGenerator"/>
    public SolutionGenerator(IProjectFileLoader projectFileLoader, ProjectSuffixFinder projectSuffixFinder)
    {
        _projectFileLoader = projectFileLoader;
        _projectSuffixFinder = projectSuffixFinder;
    }

    /// <summary>
    /// </summary>
    /// <param name="solutionFile"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    public IEnumerable<XmlSolutionFile> GenerateMany(RootSolutionFile solutionFile, SolutionFiltersManifest filters) =>
        filters.SolutionDefinitions.Select(filterDefinition =>
            GenerateFromFilterDefinition(solutionFile, filters, filterDefinition)
        );

    /// <summary>
    ///     Generates a solution filter for a specific target defined in the manifest file.
    /// </summary>
    /// <param name="targetSolutionFilterName"></param>
    /// <param name="solutionFile"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public XmlSolutionFile GenerateForTarget(
        string targetSolutionFilterName,
        RootSolutionFile solutionFile,
        SolutionFiltersManifest filters
    )
    {
        var targetFilter = filters.SolutionDefinitions.SingleOrDefault(filterDefinition =>
            filterDefinition.Name.Equals(targetSolutionFilterName, StringComparison.OrdinalIgnoreCase)
        );
        if (targetFilter == null)
        {
            throw new BadRequestException("Filter definition in manifest file: " + targetSolutionFilterName);
        }

        return GenerateFromFilterDefinition(solutionFile, filters, targetFilter);
    }

    #region Internal

    private XmlSolutionFile GenerateFromFilterDefinition(
        RootSolutionFile solutionFile,
        SolutionFiltersManifest manifest,
        ManifestSolutionDefinition solutionDefinition
    )
    {
        var dependencyTreeMapper = new SolutionProjectDependencyTreeMapper(
            solutionFile,
            path => _projectFileLoader.LoadOne(path)
        );
        var entryPointProjectsWithDependencies = dependencyTreeMapper.FlatMap(
            GetEntryPointSolutionProjects(solutionFile, solutionDefinition)
        );

        var autoIncludedTestProjects = GetTestProjects(
            solutionFile,
            manifest,
            solutionDefinition,
            entryPointProjectsWithDependencies
        );

        var testProjectsWithDependencies = dependencyTreeMapper.FlatMap(autoIncludedTestProjects);

        var allIncludedProjectFileNames = entryPointProjectsWithDependencies
            .Union(testProjectsWithDependencies)
            .Select(proj => proj.Path)
            .Distinct()
            .ToList();

        return new XmlSolutionFile(
            solutionDefinition.Name,
            allIncludedProjectFileNames.OrderBy(path => path).ToArray()
        );
    }

    private IEnumerable<SolutionProject> GetEntryPointSolutionProjects(
        RootSolutionFile solutionFile,
        ManifestSolutionDefinition solutionDefinition
    ) =>
        solutionFile.ProjectsInSolution.Where(proj =>
            solutionDefinition.Entrypoints.Contains(proj.Name) || solutionDefinition.Entrypoints.Contains(proj.Path)
        );

    private ISet<SolutionProject> GetTestProjects(
        RootSolutionFile solutionFile,
        SolutionFiltersManifest manifest,
        ManifestSolutionDefinition solutionDefinition,
        IEnumerable<SolutionProject> projects
    )
    {
        var combinedTestProjectPatterns = solutionDefinition
            .AutoIncludeSuffixPatterns.ToHashSet()
            .Union(manifest.AutoIncludeSuffixPatterns.ToHashSet())
            .ToHashSet();
        return _projectSuffixFinder.FindProjects(solutionFile, projects, combinedTestProjectPatterns).ToHashSet();
    }

    #endregion
}
