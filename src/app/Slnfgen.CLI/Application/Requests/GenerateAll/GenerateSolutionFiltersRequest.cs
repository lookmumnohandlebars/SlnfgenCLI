namespace Slnfgen.CLI.Application.Requests.GenerateAll;

/// <summary>
///     Request to generate solution filters based on a manifest file.
/// </summary>
public class GenerateSolutionFiltersRequest : IEquatable<GenerateSolutionFiltersRequest>
{
    /// <inheritdoc cref="GenerateSolutionFiltersRequest"/>
    /// <param name="manifestFilePath"></param>
    /// <param name="outputDirectory"></param>
    /// <param name="dryRun"></param>
    public GenerateSolutionFiltersRequest(string manifestFilePath, string outputDirectory, bool dryRun = false)
    {
        ManifestFilePath = manifestFilePath;
        OutputDirectory = outputDirectory;
        DryRun = dryRun;
    }

    /// <summary>
    ///     Indicates whether the request is a dry run. If true, the operation will not write any files,
    /// </summary>
    public bool DryRun { get; }

    /// <summary>
    ///     File path to the solution filters manifest file.
    ///     This file defines the solution filters to be generated.
    /// </summary>
    public string ManifestFilePath { get; }

    /// <summary>
    ///     The directory where the generated solution filters will be saved.
    ///     If not specified, defaults to the current directory.
    /// </summary>
    public string OutputDirectory { get; }

    /// <inheritdoc />
    public bool Equals(GenerateSolutionFiltersRequest? other)
    {
        if (other is null)
            return false;
        return ManifestFilePath == other.ManifestFilePath && OutputDirectory == other.OutputDirectory;
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
    public override int GetHashCode() => HashCode.Combine(ManifestFilePath, OutputDirectory);
}
