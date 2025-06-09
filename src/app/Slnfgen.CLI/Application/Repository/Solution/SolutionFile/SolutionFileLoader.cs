using Microsoft.Build.Construction;
using Slnfgen.Application.Features.Solution;

namespace Slnfgen.CLI.Domain.Solution.File.Loader;

/// <summary>
///
/// </summary>
public class SolutionFileLoader : ISolutionLoader
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public RootSolutionFile Load(string location)
    {
        if (
            location.EndsWith(".sln", StringComparison.CurrentCultureIgnoreCase)
            || location.EndsWith(".slnx", StringComparison.CurrentCultureIgnoreCase)
        )
        {
            var normalizedPath = System.IO.Path.GetFullPath(location);
            return new(SolutionFile.Parse(normalizedPath), location);
        }

        throw new ArgumentException($"Invalid solution file path: {location}. Must end with .sln or slnx");
    }
}
