using System.Text.Json.Serialization;

namespace Slnfgen.Application.Features.SolutionFilter;

/// <summary>
///     The `.slnf` file as required by .NET
/// </summary>
[Serializable]
public class SolutionFilter
{
    /// <summary>
    ///     File extension for solution filter files
    /// </summary>
    public static readonly string FileExtension = "slnf";

    /// <inheritdoc cref="SolutionFilter" />
    /// <param name="name"></param>
    /// <param name="solution"></param>
    public SolutionFilter(string name, SolutionFiltersManifestSolutionDefinition solution)
    {
        Name = name;
        Solution = solution;
    }

    /// <summary>
    /// </summary>
    [JsonIgnore]
    public string? Name { get; set; }

    /// <summary>
    /// </summary>
    [JsonPropertyName("solution")]
    public SolutionFiltersManifestSolutionDefinition Solution { get; }

    /// <summary>
    ///     The name of the solution filter file, which is the name of the filter with the `.slnf` extension
    /// </summary>
    public string GetFileName()
    {
        return $"{Name}.{FileExtension}";
    }
}
