using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.Project.Models;

namespace Slnfgen.CLI.Domain.Solution.Project.Services;

internal class SolutionProjectDependencyTreeMapper
{
    private readonly RootSolutionFile _solutionFile;
    private readonly Func<string, ProjectFile> _projectFileLoaderFunc;

    public SolutionProjectDependencyTreeMapper(
        RootSolutionFile solutionFile,
        Func<string, ProjectFile> projectFileLoaderFunc
    )
    {
        _solutionFile = solutionFile;
        _projectFileLoaderFunc = projectFileLoaderFunc;
    }

    public ISet<SolutionProject> FlatMap(SolutionProject solutionProject) => FlatMap([solutionProject]);

    public ISet<SolutionProject> FlatMap(IEnumerable<SolutionProject> projects)
    {
        var stack = new Stack<SolutionProject>(projects);
        var includedProjects = new HashSet<SolutionProject>();
        while (stack.TryPop(out var currentProject))
        {
            includedProjects.Add(currentProject);
            var dependenciesInProjectFile = _projectFileLoaderFunc(
                Path.Combine(_solutionFile.AbsolutePath, "..", currentProject.Path)
            )
                .ProjectDependencies.Select(NormalizePath)
                .ToList();
            var dependantProjectsInSolution = _solutionFile
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

    private static string NormalizePath(string path)
    {
        return path.Replace("..\\", string.Empty).Replace("/", "\\"); //TODO: this is a temporary fix, should be handled by the loader
    }
}
