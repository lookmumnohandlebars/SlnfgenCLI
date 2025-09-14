namespace Slnfgen.CLI.Application.Requests.GenerateTarget;

/// <summary>
///     Response for the GenerateTargetSolutionFilterRequest.
///     Contains the generated solution filter file name.
/// </summary>
public class GenerateTargetSolutionFilterResponse : IEquatable<GenerateTargetSolutionFilterResponse>
{
    /// <inheritdoc cref="GenerateTargetSolutionFilterResponse"/>
    /// <param name="generatedFilter"></param>
    public GenerateTargetSolutionFilterResponse(string generatedFilter)
    {
        GeneratedFilter = generatedFilter;
    }

    /// <summary>
    ///   The generated solution filter file name
    /// </summary>
    public string GeneratedFilter { get; }

    /// <inheritdoc />
    public bool Equals(GenerateTargetSolutionFilterResponse? other)
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
        return Equals((GenerateTargetSolutionFilterResponse)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => GeneratedFilter.GetHashCode();
}
