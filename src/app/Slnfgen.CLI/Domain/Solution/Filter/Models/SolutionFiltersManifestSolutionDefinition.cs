using System.ComponentModel.DataAnnotations;
using Slnfgen.CLI.Common.Paths;
using Throw;

namespace Slnfgen.CLI.Domain.Solution.Filter.Models;

/// <summary>
///     Solution definition for the solution filter file.
/// </summary>
public class SolutionFiltersSolutionDefinition
{
    /// <inheritdoc cref="SolutionFiltersSolutionDefinition"/>
    /// <param name="path">Path to the solution file from this filter</param>
    /// <param name="projects">Projects included in the solution filter</param>
    public SolutionFiltersSolutionDefinition(string path, string[] projects)
    {
        Path = PathUtilities.NormalizePathToBackslashes(path.ThrowIfNull().IfEmpty());
        Projects = projects
            .Throw()
            .IfContains(string.Empty)
            .Value.Select(PathUtilities.NormalizePathToBackslashes)
            .ToArray();
    }

    /// <summary>
    ///     Represents the path to the solution file, relative to the solution filters manifest file.
    /// </summary>
    [Required]
    public string Path { get; }

    /// <summary>
    ///     Projects that are part of the solution filter
    /// </summary>
    [Required]
    public string[] Projects { get; }
}
