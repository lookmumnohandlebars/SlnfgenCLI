using System.ComponentModel.DataAnnotations;

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
        TestProjectPatterns = [];
    }

    /// <inheritdoc cref="SolutionFiltersManifestFilterDefinition" />
    /// <param name="name">Solution filter name</param>
    /// <param name="entrypoints">Paths to entrypoint projects</param>
    /// <param name="testProjectPatterns">Test project patterns to include in the filter</param>
    public SolutionFiltersManifestFilterDefinition(
        string name,
        string[] entrypoints,
        string[]? testProjectPatterns = null
    )
    {
        Name = name;
        Entrypoints = entrypoints.Select(NormalizePath).ToArray();
        TestProjectPatterns = testProjectPatterns ?? [];
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
        set { EntryPointsRaw = value.Select(x => NormalizePath(x)).ToArray(); }
    }

    /// <summary>
    ///     Test project patterns to include in the solution filter.
    /// </summary>
    public string[] TestProjectPatterns { get; set; }

    private string NormalizePath(string entrypoint)
    {
        if (string.IsNullOrEmpty(entrypoint))
            throw new ArgumentException("Entrypoint must not be empty", nameof(entrypoint));

        // Normalize path for JSON serialization
        return entrypoint.Replace('/', '\\').Replace("\\\\", "\\");
    }
}
