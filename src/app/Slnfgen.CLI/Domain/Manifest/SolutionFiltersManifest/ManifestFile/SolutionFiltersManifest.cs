using System.ComponentModel.DataAnnotations;

namespace Slnfgen.Application.Domain.Filters;

/// <summary>
///
/// </summary>
[Serializable]
public class SolutionFiltersManifest
{
    /// <summary>
    ///
    /// </summary>
    public SolutionFiltersManifest()
    {
        FilterDefinitions = new List<SolutionFiltersManifestFilterDefinition>();
        SolutionFile = string.Empty;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="solutionFile"></param>
    /// <param name="filterDefinitions"></param>
    public SolutionFiltersManifest(string solutionFile, List<SolutionFiltersManifestFilterDefinition> filterDefinitions)
    {
        SolutionFile = solutionFile;
        FilterDefinitions = filterDefinitions;
    }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string SolutionFile { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public List<SolutionFiltersManifestFilterDefinition> FilterDefinitions { get; set; }
}
