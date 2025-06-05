using System.Text.Json.Serialization;

namespace Slnfgen.Application.Features.SolutionFilter;

/// <summary>
///     The `.slnf` file as required by .NET
///     NO
/// </summary>
[Serializable]
public class SolutionFilter
{
    public SolutionFilter(string name, SolutionFilterSolutionDefinition solution)
    {
        Name = name;
        Solution = solution;
    }
    
    [JsonIgnore]
    public string Name { get; set; }

    [JsonPropertyName("solution")]
    public SolutionFilterSolutionDefinition Solution { get; }
}