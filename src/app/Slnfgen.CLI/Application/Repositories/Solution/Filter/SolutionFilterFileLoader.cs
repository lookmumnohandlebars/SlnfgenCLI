using System.Text.Json;
using Slnfgen.CLI.Domain.Solution.Filter.Models;

namespace Slnfgen.CLI.Application.Repositories.Solution.Filter;

/// <summary>
///     Loads solution filter files from a specified directory or a single file.
///     It deserializes the JSON content of the solution filter files into `SolutionFilter.SolutionFilter` objects.
///     The solution filter files should have the `.slnf` extension.
///     If the file cannot be deserialized, an `InvalidOperationException` is thrown.
/// </summary>
public class SolutionFilterFileLoader
{
    /// <summary>
    ///     Loads a single solution filter file from the specified file path.
    ///     The file should have the `.slnf` extension.
    ///     If the file cannot be deserialized, an `InvalidOperationException` is thrown
    /// </summary>
    /// <param name="filePath">relative or file path.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public SolutionFilter LoadOne(string filePath)
    {
        using var fileStream = System.IO.File.OpenRead(filePath);
        var solutionFilter =
            JsonSerializer.Deserialize<SolutionFilter>(fileStream, SolutionFilterFileWriter.JsonOptions())
            ?? throw new InvalidOperationException($"Failed to deserialize solution filter from file: {filePath}");
        solutionFilter.Name = Path.GetFileName(filePath)
            .Replace(".slnf", string.Empty, StringComparison.OrdinalIgnoreCase);
        if (solutionFilter == null)
            throw new InvalidOperationException($"Failed to deserialize solution filter from file: {filePath}");

        return solutionFilter;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <returns></returns>
    public IEnumerable<SolutionFilter> LoadMany(string directoryPath)
    {
        return Directory.GetFiles(directoryPath, "*.slnf").Select(LoadOne);
    }
}
