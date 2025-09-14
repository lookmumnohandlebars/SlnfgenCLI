namespace Slnfgen.CLI.Application.Requests.GenerateTarget;

/// <summary>
///     Request to generate target solution filter from manifest file
/// </summary>
public class GenerateTargetSolutionFilterRequest : IEquatable<GenerateTargetSolutionFilterRequest>
{
    /// <inheritdoc cref="GenerateTargetSolutionFilterRequest"/>
    /// <param name="manifestFilePath">The file path for the manifest file</param>
    /// <param name="target">target solution filter in the manifest file</param>
    /// <param name="outputDirectory">directory to write to</param>
    /// <param name="dryRun">set to true to bypass writing files</param>
    public GenerateTargetSolutionFilterRequest(
        string manifestFilePath,
        string target,
        string outputDirectory,
        bool dryRun = false
    )
    {
        ManifestFilePath = manifestFilePath;
        OutputDirectory = outputDirectory;
        DryRun = dryRun;
        Target = target;
    }

    /// <summary>
    ///     if true, skips writing the files
    /// </summary>
    public bool DryRun { get; }

    /// <summary>
    ///
    /// </summary>
    public string ManifestFilePath { get; }

    /// <summary>
    ///     Directory to write solution filters to
    /// </summary>
    public string OutputDirectory { get; }

    /// <summary>
    ///     Target solution filter to generate from the manifest file
    /// </summary>
    public string Target { get; }

    /// <inheritdoc />
    public bool Equals(GenerateTargetSolutionFilterRequest? other)
    {
        if (other is null)
            return false;
        return ManifestFilePath == other.ManifestFilePath
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
        return Equals((GenerateTargetSolutionFilterRequest)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(ManifestFilePath, OutputDirectory);
}
