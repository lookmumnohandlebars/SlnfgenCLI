using System.Text.Json;

namespace Slnfgen.Application.Features.SolutionFilterGeneration;

/// <summary>
/// </summary>
public class SolutionFilterFileWriter : ISolutionFilterWriter
{
    /// <summary>
    /// </summary>
    /// <param name="solutionFilter"></param>
    /// <param name="outDirectory"></param>
    public string Write(SolutionFilter.SolutionFilter solutionFilter, string outDirectory)
    {
        var json = JsonSerializer.Serialize(solutionFilter, new JsonSerializerOptions());
        // Todo: Assert string paths don't have forward slashes or single backslashes
        var filePath = Path.Combine(outDirectory, solutionFilter.GetFileName());
        if (!Directory.Exists(outDirectory))
            Directory.CreateDirectory(outDirectory);
        File.WriteAllText(filePath, json);
        return filePath;
    }

    /// <summary>
    /// </summary>
    /// <param name="solutionFilters"></param>
    /// <param name="outDirectory"></param>
    public IEnumerable<string> WriteMany(
        IEnumerable<SolutionFilter.SolutionFilter> solutionFilters,
        string outDirectory
    )
    {
        var filePaths = new HashSet<string>();
        foreach (var solutionFilter in solutionFilters)
            filePaths.Add(Write(solutionFilter, outDirectory));
        return filePaths;
    }
}
