using Slnfgen.CLI.Domain.Solution.Project.Models;

namespace Slnfgen.CLI.Domain.Solution.Project.Repositories;

/// <summary>
///     Interface for loading project files (.csproj).
///     Provides methods to load a single project file or multiple project files from a directory.
/// </summary>
public interface IProjectFileLoader
{
    /// <summary>
    ///     Loads a single project file from the specified location.
    /// </summary>
    /// <param name="location">Absolute or relative path to the project</param>
    /// <returns>Project file in memory</returns>
    public ProjectFile LoadOne(string location);

    /// <summary>
    ///     Loads all project files from the specified directory.
    /// </summary>
    /// <param name="directory">directory to load all from</param>
    /// <returns>collection of project files in memory</returns>
    public IEnumerable<ProjectFile> LoadFromDirectory(string directory);
}
