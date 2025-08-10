namespace Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Repository;

/// <summary>
///     Interface for loading solution filters manifest files
/// </summary>
public interface ISolutionFiltersManifestLoader
{
    /// <summary>
    ///     Loads a solution filters manifest file from the specified location.
    /// </summary>
    /// <param name="location">Path (relative or absolute)</param>
    /// <returns>Manifest file in memory</returns>
    Models.SolutionFiltersManifest Load(string location);
}
