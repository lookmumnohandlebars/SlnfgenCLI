using System.Text.Json.Serialization;
using Throw;

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
    public SolutionFilter(string? name, SolutionFiltersManifestSolutionDefinition solution)
    {
        Name = name?.Throw("Name must not be empty").IfEmpty();
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
        if (string.IsNullOrEmpty(Name))
            throw new InvalidOperationException("Name must not be empty");
        return $"{Name}.{FileExtension}";
    }
}
