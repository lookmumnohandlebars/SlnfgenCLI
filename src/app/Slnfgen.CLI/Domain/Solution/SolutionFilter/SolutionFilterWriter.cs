using System.Text.Json;

namespace Slnfgen.Application.Features.SolutionFilterGeneration;

public class SolutionFilterWriter
{
    public void Write(SolutionFilter.SolutionFilter solutionFilter, string outDirectory)
    {
        var json = JsonSerializer.Serialize(solutionFilter);
        File.WriteAllText(Path.Combine(outDirectory, $"{solutionFilter.Name}.slnf"), json);
    }
    
    public void WriteMany(IEnumerable<SolutionFilter.SolutionFilter> solutionFilters, string outDirectory)
    {
        foreach (var solutionFilter in solutionFilters)
        {
            Write(solutionFilter, outDirectory);
        }
    }
}