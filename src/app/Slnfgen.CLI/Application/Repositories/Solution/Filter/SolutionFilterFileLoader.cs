using System.Text.Json;

namespace Slnfgen.Application.Features.SolutionFilterGeneration;

/// <summary>
///
/// </summary>
public class SolutionFilterFileLoader
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public SolutionFilter.SolutionFilter LoadOne(string filePath)
    {
        using var fileStream = File.OpenRead(filePath);
        var solutionFilter =
            JsonSerializer.Deserialize<SolutionFilter.SolutionFilter>(fileStream)
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
    public IEnumerable<SolutionFilter.SolutionFilter> LoadMany(string directoryPath)
    {
        return Directory.GetFiles(directoryPath, "*.slnf").Select(LoadOne);
    }
}
