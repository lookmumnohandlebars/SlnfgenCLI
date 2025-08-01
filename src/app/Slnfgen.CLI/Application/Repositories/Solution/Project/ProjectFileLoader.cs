using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Slnfgen.CLI.Domain.Solution.Project;
using Slnfgen.CLI.Domain.Solution.Project.Repositories;

namespace Slnfgen.CLI.Application.Repository.Solution.Project;

/// <summary>
/// </summary>
public class ProjectFileLoader : IProjectFileLoader
{
    /// <inheritdoc />
    public ProjectFile LoadOne(string location)
    {
        if (!location.EndsWith(ProjectFile.FileExtension, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"Invalid project file path: {location}. Must end with .csproj");

        var project =
            ProjectRootElement.Open(location, new ProjectCollection())
            ?? throw new ArgumentException($"Invalid project file path: {location}");
        return new ProjectFile(project);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public IEnumerable<ProjectFile> LoadFromDirectory(string directory)
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Directory not found: {directory}");

        return new Matcher()
            .AddInclude("**/*.csproj")
            .Execute(new DirectoryInfoWrapper(new DirectoryInfo(directory)))
            .Files.Select(fileMatch => fileMatch.Path)
            .Select(path => LoadOne(Path.Combine(directory, path)));
    }
}
