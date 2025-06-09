using System.Text.Json;

namespace Slnfgen.Application.Features.SolutionFilterGeneration;

/// <summary>
///
/// </summary>
public class SolutionFilterFileWriter : ISolutionFilterWriter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="solutionFilter"></param>
    /// <param name="outDirectory"></param>
    public void Write(SolutionFilter.SolutionFilter solutionFilter, string outDirectory)
    {
        var json = JsonSerializer.Serialize(solutionFilter);
        File.WriteAllText(Path.Combine(outDirectory, $"{solutionFilter.Name}.slnf"), json);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="solutionFilters"></param>
    /// <param name="outDirectory"></param>
    public void WriteMany(IEnumerable<SolutionFilter.SolutionFilter> solutionFilters, string outDirectory)
    {
        foreach (var solutionFilter in solutionFilters)
        {
            Write(solutionFilter, outDirectory);
        }
    }
}
