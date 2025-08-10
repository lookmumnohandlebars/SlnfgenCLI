using System.Text.Json.Serialization;
using Throw;

namespace Slnfgen.CLI.Domain.Solution.Filter.Models;

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
    /// <param name="name">Name of the solution filter (file name without extension)</param>
    /// <param name="solution">The full solution filter json definition</param>
    public SolutionFilter(string? name, SolutionFiltersSolutionDefinition solution)
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
    public SolutionFiltersSolutionDefinition Solution { get; }

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
