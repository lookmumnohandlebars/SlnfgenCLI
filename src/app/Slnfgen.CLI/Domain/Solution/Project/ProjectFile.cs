using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace Slnfgen.CLI.Domain.Solution.Project;

public class ProjectFile
{
    private readonly ProjectRootElement _projectRootElement;

    public ProjectFile(ProjectRootElement projectRootElement)
    {
        _projectRootElement = projectRootElement;
    }
    
    public string FullPath => _projectRootElement.FullPath;
    
    public IEnumerable<string> ProjectDependencies => _projectRootElement.Items
        .Where(item => item.ItemType == "ProjectReference")
        .Select(item => item.Include);

    public static ProjectFile LoadFromFile(string projectFilePath)
    {
        if (!projectFilePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Invalid project file path: {projectFilePath}. Must end with .csproj");
        }

        var project = ProjectRootElement.Open(projectFilePath, new ProjectCollection()) ?? throw new ArgumentException($"Invalid project file path: {projectFilePath}");
        return new(project);
    }
}