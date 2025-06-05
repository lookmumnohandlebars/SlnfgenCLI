namespace Slnfgen.CLI.Application.Features.SolutionFilter.Requests;

public class GenerateSolutionFiltersRequest
{
    public GenerateSolutionFiltersRequest(string solutionFilePath, string filtersConfigFilePath, string outputDirectory)
    {
        SolutionFilePath = solutionFilePath;
        FiltersConfigFilePath = filtersConfigFilePath;
        OutputDirectory = outputDirectory;
    }

    public string SolutionFilePath { get; set; }
    public string FiltersConfigFilePath { get; set; }
    public string OutputDirectory { get; set; }
}