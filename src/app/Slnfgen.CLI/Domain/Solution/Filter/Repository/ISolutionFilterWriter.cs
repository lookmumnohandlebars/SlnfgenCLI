using Slnfgen.CLI.Domain.Solution.Filter.Models;

namespace Slnfgen.CLI.Domain.Solution.Filter.Repository;

/// <summary>
///     Interface for writing solution filter files
/// </summary>
public interface ISolutionFilterWriter
{
    /// <summary>
    ///     Writes a solution filter to the specified directory.
    ///     The file name is derived from the filter's name with the `.slnf` extension.
    ///     If the directory does not exist, it will be created.
    /// </summary>
    /// <param name="solutionFilter">defined solution filter</param>
    /// <param name="outDirectory">directory to write to</param>
    string Write(SolutionFilter solutionFilter, string outDirectory);

    /// <summary>
    ///     Writes multiple solution filters to the specified directory.
    ///     Each filter will be serialized to JSON and saved with the `.slnf`
    ///     extension. If the directory does not exist, it will be created.
    /// </summary>
    /// <param name="solutionFilters">defined solution filters</param>
    /// <param name="outDirectory">directory to write to</param>
    IEnumerable<string> WriteMany(IEnumerable<SolutionFilter> solutionFilters, string outDirectory);
}
