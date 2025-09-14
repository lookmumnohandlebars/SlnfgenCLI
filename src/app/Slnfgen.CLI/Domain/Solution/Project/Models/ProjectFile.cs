using Microsoft.Build.Construction;
using Slnfgen.CLI.Common.Paths;

namespace Slnfgen.CLI.Domain.Solution.Project.Models;

/// <summary>
///     A wrapper for a project file (.csproj) that provides access to its properties and project references.
/// </summary>
public class ProjectFile : IEquatable<ProjectFile>
{
    /// <summary>
    ///     The file extension for project files - only .csproj files are supported
    /// </summary>
    public static readonly string FileExtension = ".csproj";

    private readonly ProjectRootElement _projectRootElement;

    /// <inheritdoc cref="ProjectFile" />
    public ProjectFile(ProjectRootElement projectRootElement)
    {
        _projectRootElement = projectRootElement;
    }

    /// <summary>
    ///     The absolute path of the project file
    /// </summary>
    public string FullPath => _projectRootElement.FullPath;

    /// <summary>
    ///     All project (path) references defined in the project file
    /// </summary>
    public IEnumerable<string> ProjectDependencies =>
        _projectRootElement.Items.Where(item => item.ItemType == "ProjectReference").Select(item => item.Include);

    /// <inheritdoc />
    public bool Equals(ProjectFile? other)
    {
        if (other is null)
            return false;
        return FullPath.Equals(other.FullPath);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        return Equals((ProjectFile)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return FullPath.GetHashCode();
    }
}
