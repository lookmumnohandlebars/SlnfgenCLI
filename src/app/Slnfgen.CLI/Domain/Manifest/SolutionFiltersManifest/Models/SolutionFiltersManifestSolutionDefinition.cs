using System.ComponentModel.DataAnnotations;
using Throw;

namespace Slnfgen.Application.Features.SolutionFilter;

/// <summary>
/// </summary>
public class SolutionFiltersManifestSolutionDefinition
{
    /// <summary>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="projects"></param>
    public SolutionFiltersManifestSolutionDefinition(string path, string[] projects)
    {
        Path = FormatPathForJson(path.Throw().IfEmpty());
        Projects = projects.Throw().IfContains(string.Empty).Value.Select(FormatPathForJson).ToArray();
    }

    /// <summary>
    /// </summary>
    [Required]
    public string Path { get; }

    /// <summary>
    /// </summary>
    [Required]
    public string[] Projects { get; }

    private string FormatPathForJson(string path) => path.Replace("/", "\\\\");
    // .Replace("\\", "\\\\");
}
