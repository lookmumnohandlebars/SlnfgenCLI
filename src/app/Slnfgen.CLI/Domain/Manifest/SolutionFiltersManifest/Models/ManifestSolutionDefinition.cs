using System.ComponentModel.DataAnnotations;

namespace Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;

/// <summary>
///
/// </summary>
public class ManifestSolutionDefinition
{
    /// <inheritdoc cref="ManifestSolutionDefinition" />
    public ManifestSolutionDefinition()
    {
        Name = string.Empty;
        Entrypoints = [];
        AutoIncludeSuffixPatterns = [];
    }

    /// <inheritdoc cref="ManifestSolutionDefinition" />
    /// <param name="name">Solution name</param>
    /// <param name="entrypoints">Paths to entrypoint projects</param>
    /// <param name="autoIncludeSuffixPatterns">project suffix patterns to automatically include in the filter</param>
    public ManifestSolutionDefinition(string name, string[] entrypoints, string[]? autoIncludeSuffixPatterns = null)
    {
        Name = name;
        Entrypoints = entrypoints.Select(NormalizePath).ToArray();
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
        set { EntryPointsRaw = value.Select(x => NormalizePath(x)).ToArray(); }
    }

    /// <summary>
    ///     Test project patterns to include in the solution filter.
    /// </summary>
    public string[] AutoIncludeSuffixPatterns { get; set; }

    private string NormalizePath(string entrypoint)
    {
        if (string.IsNullOrEmpty(entrypoint))
            throw new ArgumentException("Entrypoint must not be empty", nameof(entrypoint));

        // Normalize path for JSON serialization
        return entrypoint.Replace('/', '\\');
    }
}
