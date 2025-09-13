using System.ComponentModel.DataAnnotations;
using Slnfgen.CLI.Common.Paths;

namespace Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;

/// <summary>
///     The manifest definition for a single solution filter
/// </summary>
[Serializable]
public class SolutionFiltersManifestFilterDefinition
{
    /// <inheritdoc cref="SolutionFiltersManifestFilterDefinition" />
    public SolutionFiltersManifestFilterDefinition()
    {
        Name = string.Empty;
        Entrypoints = [];
        AutoIncludeSuffixPatterns = [];
    }

    /// <inheritdoc cref="SolutionFiltersManifestFilterDefinition" />
    /// <param name="name">Solution filter name</param>
    /// <param name="entrypoints">Paths to entrypoint projects</param>
    /// <param name="autoIncludeSuffixPatterns">project suffix patterns to automatically include in the filter</param>
    public SolutionFiltersManifestFilterDefinition(
        string name,
        string[] entrypoints,
        string[]? autoIncludeSuffixPatterns = null
    )
    {
        Name = name;
        Entrypoints = entrypoints;
        AutoIncludeSuffixPatterns = autoIncludeSuffixPatterns ?? [];
    }

    /// <summary>
    ///     Name of the solution filter, used for the file name
    ///     and for the filter definition in the solution file.
    ///     Must be unique within the solution.
    /// </summary>
    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    private string[] EntryPointsRaw { get; set; } = [];

    /// <summary>
    ///     Entry point projects for the solution filter.
    ///     These are the projects that will be included in the solution filter
    ///     and searched for underlying references.
    /// </summary>
    [Required]
    [MinLength(1)]
    public string[] Entrypoints
    {
        get => EntryPointsRaw;
        set
        {
            EntryPointsRaw = value
                .Select(PathUtilities.NormalizePathToBackslashes)
                .Select(TryAddProjectFileIfNotIncluded)
                .ToArray();
        }
    }

    /// <summary>
    ///     Test project patterns to include in the solution filter.
    /// </summary>
    public string[] AutoIncludeSuffixPatterns { get; set; }

    /// <summary>
    ///     Utility to add a project file if a project directory is provided
    /// </summary>
    /// <param name="projectFilePath"></param>
    /// <returns></returns>
    public static string TryAddProjectFileIfNotIncluded(string projectFilePath)
    {
        if (projectFilePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
            return projectFilePath;
        var suspectedProject = projectFilePath.Split('\\').LastOrDefault();
        if (suspectedProject != null)
            return $@"{projectFilePath}\{suspectedProject}.csproj";
        return projectFilePath;
    }
}
