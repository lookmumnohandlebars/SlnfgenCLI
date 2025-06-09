using Slnfgen.Application.Features.SolutionFilter;
using Slnfgen.Application.Features.SolutionFilterGeneration;

namespace Slnfgen.CLI.TestImplementations.Application.Repository;

public class FakeSolutionFilterWriter : ISolutionFilterWriter
{
    public Dictionary<string, SolutionFilter> Store = new();

    public void Write(SolutionFilter solutionFilter, string outDirectory)
    {
        Store.Add(Path.Combine(outDirectory, solutionFilter.Name), solutionFilter);
    }

    public void WriteMany(IEnumerable<SolutionFilter> solutionFilters, string outDirectory)
    {
        foreach (var solutionFilter in solutionFilters)
            Write(solutionFilter, outDirectory);
    }
}
