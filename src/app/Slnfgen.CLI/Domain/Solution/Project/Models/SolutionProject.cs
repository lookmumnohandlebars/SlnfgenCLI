using Microsoft.Build.Construction;

namespace Slnfgen.CLI.Domain.Solution.Project.Models;

/// <summary>
///     Wrapper for a project in a solution file.
/// </summary>
public class SolutionProject : IEquatable<SolutionProject>
{
    private readonly ProjectInSolution _projectInSolution;

    /// <inheritdoc cref="SolutionProject"/>
    /// <param name="projectInSolution"></param>
    public SolutionProject(ProjectInSolution projectInSolution)
    {
        _projectInSolution = projectInSolution;
    }

    /// <summary>
    ///     Project file name, which is the name of the project file without the extension.
    /// </summary>
    public string Name => _projectInSolution.ProjectName;

    /// <summary>
    ///     Path to the project file, relative to the solution file.
    /// </summary>
    public string Path => NormalizedPath(_projectInSolution.RelativePath);

    /// <inheritdoc />
    public bool Equals(SolutionProject? other)
    {
        if (other is null)
            return false;
        return _projectInSolution.AbsolutePath == other._projectInSolution.AbsolutePath
            && _projectInSolution.ProjectGuid == other._projectInSolution.ProjectGuid;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        return Equals((SolutionProject)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(_projectInSolution.AbsolutePath);
    }

    private string NormalizedPath(string path)
    {
        return path.Replace('/', '\\');
    }
}
