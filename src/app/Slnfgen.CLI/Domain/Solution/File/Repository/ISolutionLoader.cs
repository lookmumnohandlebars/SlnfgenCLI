using Slnfgen.Application.Features.Solution;

namespace Slnfgen.CLI.Domain.Solution.File.Loader;

/// <summary>
/// </summary>
public interface ISolutionLoader
{
    /// <summary>
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public RootSolutionFile Load(string location);
}
