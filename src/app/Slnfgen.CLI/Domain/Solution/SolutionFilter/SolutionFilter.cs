using System.Text.Json.Serialization;

namespace Slnfgen.Application.Features.SolutionFilter;

/// <summary>
///     The `.slnf` file as required by .NET
///     NO
/// </summary>
[Serializable]
public class SolutionFilter
{
    /// <inheritdoc cref="SolutionFilter"/>
    /// <param name="name"></param>
    /// <param name="solution"></param>
    public SolutionFilter(string name, SolutionFiltersManifestSolutionDefinition solution)
    {
        Name = name;
        Solution = solution;
    }

    /// <summary>
    ///
    /// </summary>
    [JsonIgnore]
    public string Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("solution")]
    public SolutionFiltersManifestSolutionDefinition Solution { get; }
}
