using System.ComponentModel.DataAnnotations;

namespace Slnfgen.Application.Domain.Filters;

[Serializable]
public class FilterDefinition
{
    public FilterDefinition()
    {
        Name = string.Empty;
        Entrypoints = [];
    }
    
    public FilterDefinition(string name, string[] entrypoints)
    {
        Name = name;
        Entrypoints = entrypoints;
    }

    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    [Required]
    [MinLength(1)]
    public string[] Entrypoints { get; set; }
}