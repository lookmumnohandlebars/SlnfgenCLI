using Microsoft.Build.Construction;

namespace Slnfgen.CLI.Domain.Solution.Project.Models;

/// <summary>
///     A wrapper for a project file (.csproj) that provides access to its properties and project references.
/// </summary>
public class ProjectFile
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
}
