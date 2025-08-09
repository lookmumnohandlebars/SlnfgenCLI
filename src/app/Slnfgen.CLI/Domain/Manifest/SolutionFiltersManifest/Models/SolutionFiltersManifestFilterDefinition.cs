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
        Entrypoints = entrypoints.Select(NormalizePath).ToArray();
    }

    /// <summary>
    /// </summary>
    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    private string[] _entryPointsRaw { get; set; } = [];

    /// <summary>
    /// </summary>
    [Required]
    [MinLength(1)]
    public string[] Entrypoints
    {
        get => _entryPointsRaw;
        set { _entryPointsRaw = value.Select(x => NormalizePath(x)).ToArray(); }
    }

    private string NormalizePath(string entrypoint)
    {
        if (string.IsNullOrEmpty(entrypoint))
            throw new ArgumentException("Entrypoint must not be empty", nameof(entrypoint));

        // Normalize path for JSON serialization
        return entrypoint.Replace('/', '\\').Replace("\\\\", "\\");
    }
}
