namespace Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;

/// <summary>
/// </summary>
public class GenerateSolutionFilterResponse : IEquatable<GenerateSolutionFilterResponse>
{
    /// <summary>
    /// </summary>
    /// <param name="generatedFilter"></param>
    public GenerateSolutionFilterResponse(string generatedFilter)
    {
        GeneratedFilter = generatedFilter;
    }

    /// <summary>
    /// </summary>
    public string GeneratedFilter { get; }

    /// <inheritdoc />
    public bool Equals(GenerateSolutionFilterResponse? other)
    {
        if (other is null)
            return false;
        return GeneratedFilter.Equals(other.GeneratedFilter, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        return Equals((GenerateSolutionFilterResponse)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => GeneratedFilter.GetHashCode();
}
