using Slnfgen.CLI.Domain.Solution.File.Models;

namespace Slnfgen.CLI.Domain.Solution.File.Repository;

/// <summary>
///     Interface for loading solution files
/// </summary>
public interface ISolutionLoader
{
    /// <summary>
    ///     Loads a solution file from the specified location.
    /// </summary>
    /// <param name="location">Path (absolute or relative)</param>
    /// <returns>Solution file in memory</returns>
    public RootSolutionFile Load(string location);
}
