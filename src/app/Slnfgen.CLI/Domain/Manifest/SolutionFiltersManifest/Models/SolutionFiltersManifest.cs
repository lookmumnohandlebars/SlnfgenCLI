using System.ComponentModel.DataAnnotations;

namespace Slnfgen.Application.Domain.Filters;

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
    }

    /// <inheritdoc cref="SolutionFiltersManifest" />
    public SolutionFiltersManifest(string solutionFile, List<SolutionFiltersManifestFilterDefinition> filterDefinitions)
    {
        SolutionFile = solutionFile;
        FilterDefinitions = filterDefinitions;
    }

    /// <summary>
    ///     The relative path to the solution file which the manifest builds upon
    /// </summary>
    [Required]
    public string SolutionFile { get; set; }

    /// <summary>
    ///     The list of solution filters with entrypoints
    /// </summary>
    [Required]
    public List<SolutionFiltersManifestFilterDefinition> FilterDefinitions { get; set; }
}
