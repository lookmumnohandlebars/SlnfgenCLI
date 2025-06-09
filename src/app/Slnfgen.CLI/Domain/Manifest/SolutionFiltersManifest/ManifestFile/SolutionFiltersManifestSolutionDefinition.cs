using System.ComponentModel.DataAnnotations;

namespace Slnfgen.Application.Features.SolutionFilter;

/// <summary>
///
/// </summary>
public class SolutionFiltersManifestSolutionDefinition
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <param name="projects"></param>
    public SolutionFiltersManifestSolutionDefinition(string path, string[] projects)
    {
        Path = path;
        Projects = projects;
    }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string Path { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string[] Projects { get; set; }
}
