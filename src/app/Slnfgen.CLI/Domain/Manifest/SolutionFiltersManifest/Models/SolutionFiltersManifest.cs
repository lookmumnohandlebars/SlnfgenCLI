using Microsoft.Build.Framework;

namespace Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;

/// <summary>
///     The manifest definition for declaring solution filters belonging to a solution
/// </summary>
[Serializable]
public class SolutionFiltersManifest
{
    /// <inheritdoc cref="SolutionFiltersManifest" />
    public SolutionFiltersManifest()
    {
        FilterDefinitions = new List<SolutionFiltersManifestFilterDefinition>();
        SolutionFile = string.Empty;
        AutoIncludeSuffixPatterns = new List<string>();
        SolutionDefinitions = new List<ManifestSolutionDefinition>();
    }

    /// <inheritdoc cref="SolutionFiltersManifest" />
    public SolutionFiltersManifest(
        string solutionFile,
        List<SolutionFiltersManifestFilterDefinition> filterDefinitions,
        List<ManifestSolutionDefinition> solutionDefinitions,
        List<string>? autoIncludeSuffixPatterns = null
    )
    {
        SolutionFile = solutionFile;
        FilterDefinitions = filterDefinitions;
        AutoIncludeSuffixPatterns = autoIncludeSuffixPatterns ?? new List<string>();
        SolutionDefinitions = solutionDefinitions;
    }

    /// <summary>
    ///     The relative path to the solution file which the manifest builds upon
    /// </summary>
    [Required]
    public string SolutionFile { get; set; }

    /// <summary>
    ///     The list of solution filters with entrypoints
    /// </summary>
    public List<SolutionFiltersManifestFilterDefinition> FilterDefinitions { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<ManifestSolutionDefinition> SolutionDefinitions { get; set; }

    /// <summary>
    ///     project patterns to include in the solution filter.
    ///     These patterns are used to identify projects that should be included in the solution filter
    ///     based on their names or paths.
    ///     The patterns are matched against the project names and paths in the solution.
    ///     For example, patterns like "Test", "Tests", "UnitTests", etc.
    ///     This allows the solution filter generator to automatically include test projects
    ///     that match these patterns without needing to explicitly list them in the manifest.
    /// </summary>
    public List<string> AutoIncludeSuffixPatterns { get; set; }
}
