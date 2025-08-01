using Slnfgen.Application.Features.SolutionFilter;
using Slnfgen.Application.Features.SolutionFilterGeneration;

namespace Slnfgen.CLI.TestImplementations.Application.Repository;

public class FakeSolutionFilterWriter : ISolutionFilterWriter
{
    public Dictionary<string, SolutionFilter> Store = new();

    public string Write(SolutionFilter solutionFilter, string outDirectory)
    {
        var key = Path.Combine(outDirectory, solutionFilter.Name ?? string.Empty);
        Store.Add(key, solutionFilter);
        return key;
    }

    public IEnumerable<string> WriteMany(IEnumerable<SolutionFilter> solutionFilters, string outDirectory)
    {
        return solutionFilters.Select(solutionFilter => Write(solutionFilter, outDirectory)).ToList();
    }
}
