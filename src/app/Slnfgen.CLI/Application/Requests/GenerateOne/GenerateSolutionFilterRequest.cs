namespace Slnfgen.CLI.Application.Requests.GenerateOne;

/// <summary>
/// </summary>
public class GenerateSolutionFilterRequest : IEquatable<GenerateSolutionFilterRequest>
{
    /// <summary>
    /// </summary>
    /// <param name="filtersConfigFilePath"></param>
    /// <param name="target"></param>
    /// <param name="outputDirectory"></param>
    /// <param name="dryRun"></param>
    public GenerateSolutionFilterRequest(
        string filtersConfigFilePath,
        string target,
        string outputDirectory,
        bool dryRun = false
    )
    {
        FiltersConfigFilePath = filtersConfigFilePath;
        OutputDirectory = outputDirectory;
        DryRun = dryRun;
        Target = target;
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

    /// <summary>
    ///
    /// </summary>
    public string Target { get; }

    /// <inheritdoc />
    public bool Equals(GenerateSolutionFilterRequest? other)
    {
        if (other is null)
            return false;
        return FiltersConfigFilePath == other.FiltersConfigFilePath
            && OutputDirectory == other.OutputDirectory
            && Target == other.Target;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        return Equals((GenerateSolutionFilterRequest)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(FiltersConfigFilePath, OutputDirectory);
    }
}
