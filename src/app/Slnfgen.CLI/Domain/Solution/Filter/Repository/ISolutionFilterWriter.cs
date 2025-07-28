namespace Slnfgen.Application.Features.SolutionFilterGeneration;

/// <summary>
/// </summary>
public interface ISolutionFilterWriter
{
    /// <summary>
    /// </summary>
    /// <param name="solutionFilter"></param>
    /// <param name="outDirectory"></param>
    string Write(SolutionFilter.SolutionFilter solutionFilter, string outDirectory);

    /// <summary>
    /// </summary>
    /// <param name="solutionFilters"></param>
    /// <param name="outDirectory"></param>
    IEnumerable<string> WriteMany(IEnumerable<SolutionFilter.SolutionFilter> solutionFilters, string outDirectory);
}
