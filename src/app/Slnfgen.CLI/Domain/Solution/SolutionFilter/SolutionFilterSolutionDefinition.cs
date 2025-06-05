using System.ComponentModel.DataAnnotations;

namespace Slnfgen.Application.Features.SolutionFilter;

public class SolutionFilterSolutionDefinition
{
    public SolutionFilterSolutionDefinition(string path, string[] projects)
    {
        Path = path;
        Projects = projects;
    }

    [Required]
    public string Path { get; set; }
    
    [Required]
    public string[] Projects { get; set; }
}