namespace Slnfgen.CLI.Application.Features.SolutionFilter.Requests;

/// <summary>
///
/// </summary>
public class GenerateSolutionFiltersRequest : IEquatable<GenerateSolutionFiltersRequest>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="filtersConfigFilePath"></param>
    /// <param name="outputDirectory"></param>
    public GenerateSolutionFiltersRequest(string filtersConfigFilePath, string outputDirectory)
    {
        FiltersConfigFilePath = filtersConfigFilePath;
        OutputDirectory = outputDirectory;
    }

    /// <summary>
    ///
    /// </summary>
    public string FiltersConfigFilePath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string OutputDirectory { get; set; }

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
    public override int GetHashCode() => HashCode.Combine(FiltersConfigFilePath, OutputDirectory);
}
