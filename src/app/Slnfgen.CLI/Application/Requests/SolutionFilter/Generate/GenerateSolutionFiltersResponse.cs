namespace Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;

/// <summary>
/// </summary>
public class GenerateSolutionFiltersResponse : IEquatable<GenerateSolutionFiltersResponse>
{
    /// <summary>
    /// </summary>
    /// <param name="generatedFilters"></param>
    public GenerateSolutionFiltersResponse(IEnumerable<string> generatedFilters)
    {
        GeneratedFilters = generatedFilters;
    }

    /// <summary>
    /// </summary>
    public IEnumerable<string> GeneratedFilters { get; }

    /// <inheritdoc />
    public bool Equals(GenerateSolutionFiltersResponse? other)
    {
        if (other is null)
            return false;
        return GeneratedFilters.SequenceEqual(other.GeneratedFilters);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        return Equals((GenerateSolutionFiltersResponse)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => GeneratedFilters.GetHashCode();
}
