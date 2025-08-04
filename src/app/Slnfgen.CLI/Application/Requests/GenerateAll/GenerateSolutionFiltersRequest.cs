namespace Slnfgen.CLI.Application.Features.SolutionFilter.Requests;

/// <summary>
/// </summary>
public class GenerateSolutionFiltersRequest : IEquatable<GenerateSolutionFiltersRequest>
{
    /// <summary>
    /// </summary>
    /// <param name="filtersConfigFilePath"></param>
    /// <param name="outputDirectory"></param>
    /// <param name="dryRun"></param>
    public GenerateSolutionFiltersRequest(string filtersConfigFilePath, string outputDirectory, bool dryRun = false)
    {
        FiltersConfigFilePath = filtersConfigFilePath;
        OutputDirectory = outputDirectory;
        DryRun = dryRun;
    }

    /// <summary>
    /// </summary>
    public bool DryRun { get; }

    /// <summary>
    /// </summary>
    public string FiltersConfigFilePath { get; }

    /// <summary>
    /// </summary>
    public string OutputDirectory { get; }

    /// <inheritdoc />
    public bool Equals(GenerateSolutionFiltersRequest? other)
    {
        if (other is null)
            return false;
        return FiltersConfigFilePath == other.FiltersConfigFilePath && OutputDirectory == other.OutputDirectory;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        return Equals((GenerateSolutionFiltersRequest)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(FiltersConfigFilePath, OutputDirectory);
    }
}
