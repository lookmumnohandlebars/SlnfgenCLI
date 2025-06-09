using Microsoft.Build.Construction;
using Slnfgen.Application.Domain.Project;

namespace Slnfgen.Application.Features.Solution;

/// <summary>
///
/// </summary>
public class RootSolutionFile
{
    private string _path;
    private SolutionFile _solutionFile;

    /// <summary>
    ///
    /// </summary>
    /// <param name="solutionFile"></param>
    /// <param name="path"></param>
    public RootSolutionFile(SolutionFile solutionFile, string path)
    {
        _solutionFile = solutionFile;
        _path = path;
    }

    /// <summary>
    ///
    /// </summary>
    public string Path => _path;

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<SolutionProject> ProjectsInSolution =>
        _solutionFile.ProjectsInOrder.Select(proj => new SolutionProject(proj));
}
