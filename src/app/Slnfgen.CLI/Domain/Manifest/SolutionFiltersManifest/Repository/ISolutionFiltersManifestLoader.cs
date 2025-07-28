namespace Slnfgen.Application.Domain.Filters;

/// <summary>
/// </summary>
public interface ISolutionFiltersManifestLoader
{
    /// <summary>
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    SolutionFiltersManifest Load(string location);
}
