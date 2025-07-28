using System.ComponentModel.DataAnnotations;

namespace Slnfgen.Application.Domain.Filters;

/// <summary>
/// </summary>
[Serializable]
public class SolutionFiltersManifestFilterDefinition
{
    /// <summary>
    /// </summary>
    public SolutionFiltersManifestFilterDefinition()
    {
        Name = string.Empty;
        Entrypoints = [];
    }

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="entrypoints"></param>
    public SolutionFiltersManifestFilterDefinition(string name, string[] entrypoints)
    {
        Name = name;
        Entrypoints = entrypoints;
    }

    /// <summary>
    /// </summary>
    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [MinLength(1)]
    public string[] Entrypoints { get; set; }
}
