using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace Slnfgen.CLI.Domain.Solution.Project;

/// <summary>
///
/// </summary>
public class ProjectFile
{
    private readonly ProjectRootElement _projectRootElement;

    /// <inheritdoc cref="ProjectFile"/>
    public ProjectFile(ProjectRootElement projectRootElement)
    {
        _projectRootElement = projectRootElement;
    }

    /// <summary>
    ///
    /// </summary>
    public string FullPath => _projectRootElement.FullPath;

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<string> ProjectDependencies =>
        _projectRootElement.Items.Where(item => item.ItemType == "ProjectReference").Select(item => item.Include);

    /// <summary>
    ///
    /// </summary>
    /// <param name="projectFilePath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ProjectFile LoadFromFile(string projectFilePath)
    {
        if (!projectFilePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Invalid project file path: {projectFilePath}. Must end with .csproj");
        }

        var project =
            ProjectRootElement.Open(projectFilePath, new ProjectCollection())
            ?? throw new ArgumentException($"Invalid project file path: {projectFilePath}");
        return new(project);
    }
}
