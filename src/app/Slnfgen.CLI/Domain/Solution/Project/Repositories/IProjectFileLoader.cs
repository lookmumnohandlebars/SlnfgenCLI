namespace Slnfgen.CLI.Domain.Solution.Project.Repositories;

/// <summary>
/// </summary>
public interface IProjectFileLoader
{
    /// <summary>
    /// </summary>
    /// <param name="location">Absolute or relative path to the project</param>
    /// <returns></returns>
    public ProjectFile LoadOne(string location);

    /// <summary>
    ///
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public IEnumerable<ProjectFile> LoadFromDirectory(string directory);
}
