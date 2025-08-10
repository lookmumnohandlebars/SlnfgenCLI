using System.Text.Json;
using Slnfgen.CLI.Domain.Solution.Filter.Models;
using Slnfgen.CLI.Domain.Solution.Filter.Repository;

namespace Slnfgen.CLI.Application.Repositories.Solution.Filter;

/// <summary>
///     Writes solution filter files to a specified directory.
///     It serializes `SolutionFilter` objects to JSON format and saves them with the `.slnf` extension.
///     If the directory does not exist, it creates the directory.
///     The solution filter files should have the `.slnf` extension.
///     The file name is derived from the `SolutionFilter` object's name property.
///     If the file cannot be written, an `InvalidOperationException` is thrown.
/// </summary>
public class SolutionFilterFileWriter : ISolutionFilterWriter
{
    /// <summary>
    ///     Writes a single solution filter to a specified directory.
    /// </summary>
    /// <param name="solutionFilter">The solution filter</param>
    /// <param name="outDirectory">The directory to write to</param>
    public string Write(SolutionFilter solutionFilter, string outDirectory)
    {
        var json = JsonSerializer.Serialize(solutionFilter, new JsonSerializerOptions());
        // Todo: Assert string paths don't have forward slashes or single backslashes
        var filePath = Path.Combine(outDirectory, solutionFilter.GetFileName());
        if (!Directory.Exists(outDirectory))
            Directory.CreateDirectory(outDirectory);
        System.IO.File.WriteAllText(filePath, json);
        return filePath;
    }

    /// <summary>
    ///     Writes multiple solution filters to a specified directory.
    /// </summary>
    /// <param name="solutionFilters">A collection of in-memory solution filters</param>
    /// <param name="outDirectory">The directory to write to</param>
    public IEnumerable<string> WriteMany(IEnumerable<SolutionFilter> solutionFilters, string outDirectory)
    {
        var filePaths = new HashSet<string>();
        foreach (var solutionFilter in solutionFilters)
            filePaths.Add(Write(solutionFilter, outDirectory));
        return filePaths;
    }
}
