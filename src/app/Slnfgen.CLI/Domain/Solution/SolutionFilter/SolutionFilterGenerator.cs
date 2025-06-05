using Microsoft.Build.Evaluation;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Domain.Project;
using Slnfgen.Application.Features.Solution;
using Slnfgen.Application.Features.SolutionFilter;
using Slnfgen.CLI.Domain.Solution.Project;

namespace Slnfgen.Application.Features.SolutionFilterGeneration;

public class SolutionFilterGenerator
{
    public IEnumerable<SolutionFilter.SolutionFilter> GenerateMany(RootSolutionFile solutionFile, SolutionFiltersConfiguration filters) =>
        filters.FilterDefinitions
            .Select(filterDefinition => GenerateFromFilterDefinition(solutionFile, filterDefinition))
            .ToArray();

    private SolutionFilter.SolutionFilter GenerateFromFilterDefinition(
        RootSolutionFile solutionFile, 
        FilterDefinition filterDefinition)
    {
        
        var entryPointProjects = GetEntryPointSolutionProjects(solutionFile, filterDefinition).ToList();
        var entryPointProjectsWithDependencies = AggregateProjectDependencies(entryPointProjects, solutionFile);
        
        return new SolutionFilter.SolutionFilter(
            filterDefinition.Name, 
            new SolutionFilterSolutionDefinition(
                solutionFile.Path, 
                entryPointProjectsWithDependencies.ToArray()
            )
        );
    }

    private IEnumerable<SolutionProject> GetEntryPointSolutionProjects(RootSolutionFile solutionFile,
        FilterDefinition filterDefinition) =>
        solutionFile.ProjectsInSolution.Where(proj =>
            filterDefinition.Entrypoints.Contains(proj.Name) || filterDefinition.Entrypoints.Contains(proj.Path));

    private IEnumerable<string> AggregateProjectDependencies(IEnumerable<SolutionProject> projects, RootSolutionFile solutionFile)
    {
        var stack = new Stack<SolutionProject>(projects);
        var includedProjectPaths = new HashSet<string>();
        while (stack.TryPop(out var currentProject))
        {
            includedProjectPaths.Add(currentProject.Path);
            var dependenciesInProjectFile = ProjectFile.LoadFromFile(Path.Combine(solutionFile.Path, "..", currentProject.Path)).ProjectDependencies;
            var dependantProjectsInSolution =
                solutionFile.ProjectsInSolution.Where(proj => dependenciesInProjectFile.Contains(proj.Path));
            foreach (var dependantProjectInSolution in dependantProjectsInSolution.Where(proj => !includedProjectPaths.Contains(proj.Path)))
            {
                stack.Push(dependantProjectInSolution);
            }
                
        }

        return includedProjectPaths;
    }
}