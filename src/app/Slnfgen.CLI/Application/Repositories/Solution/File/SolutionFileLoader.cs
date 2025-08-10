using Microsoft.Build.Construction;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.File.Repository;

namespace Slnfgen.CLI.Application.Repositories.Solution.File;

/// <summary>
///     Loads a solution file from the specified location.
///     The file must have a .sln or .slnx extension.
///     If the file does not exist or is not a valid solution file, an exception is thrown.
/// </summary>
public class SolutionFileLoader : ISolutionLoader
{
    /// <summary>
    ///     Loads a solution file from the specified location.
    ///     The file must have a .sln or .slnx extension.
    ///     If the file does not exist or is not a valid solution file, an exception is thrown.
    ///     The solution file is parsed and returned as a <see cref="RootSolutionFile"/> object, which contains the parsed solution file and its normalized path.
    ///     The normalized path is the absolute path to the solution file, with forward slashes replaced by backslashes.
    ///     The <see cref="RootSolutionFile"/> object can be used to access the projects in the solution and their properties, such as name and path.
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
            var normalizedPath = Path.GetFullPath(location);
            return new RootSolutionFile(SolutionFile.Parse(normalizedPath), normalizedPath);
        }

        throw new ArgumentException($"Invalid solution file path: {location}. Must end with .sln or slnx");
    }
}
