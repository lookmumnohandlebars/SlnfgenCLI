using Microsoft.Build.Construction;
using Slnfgen.CLI.Domain.Solution.Project.Models;

namespace Slnfgen.CLI.Domain.Solution.File.Models;

/// <summary>
///     Wrapper around the MSBuild Solution File
/// </summary>
public class RootSolutionFile
{
    private readonly SolutionFile _solutionFile;

    /// <inheritdoc cref="RootSolutionFile" />
    /// <param name="solutionFile">The MSBuild loaded solution file</param>
    /// <param name="absolutePath"></param>
    public RootSolutionFile(SolutionFile solutionFile, string absolutePath)
    {
        if (!Path.IsPathRooted(absolutePath))
            throw new ArgumentException(
                $"Cannot load solution file path '{absolutePath}' as it is not an absolute path. Please raise an issue"
            );
        _solutionFile = solutionFile;
        AbsolutePath = absolutePath;
    }

    /// <summary>
    ///     This constructor will load the solution file from an absolute path.
    /// </summary>
    /// <param name="absolutePath"></param>
    public RootSolutionFile(string absolutePath)
        : this(SolutionFile.Parse(absolutePath), absolutePath) { }

    /// <summary>
    ///     The absolute path of the solution file.
    /// </summary>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public string AbsolutePath { get; }

    /// <summary>
    ///     The parent directory of the solution file.
    /// </summary>
    public DirectoryInfo ParentDirectory =>
        Directory.GetParent(AbsolutePath)
        ?? throw new DirectoryNotFoundException(
            "Failed to find the parent directory for solution file. Please raise an issue"
        );

    /// <summary>
    ///     The file name (path excluded)
    /// </summary>
    /// <exception cref="Exception"></exception>
    public string FileName =>
        Path.GetFileName(AbsolutePath)
        ?? throw new Exception($"Failed to get the file name from '{AbsolutePath}'. Please raise an issue");

    /// <summary>
    ///     Gets the projects declared in the solution file.
    /// </summary>
    public IEnumerable<SolutionProject> ProjectsInSolution =>
        _solutionFile.ProjectsInOrder.Select(proj => new SolutionProject(proj));
}
