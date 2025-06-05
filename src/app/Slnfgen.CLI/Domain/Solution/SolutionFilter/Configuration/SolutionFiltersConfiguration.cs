using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using YamlDotNet.Serialization;

namespace Slnfgen.Application.Domain.Filters;

[Serializable]
public class SolutionFiltersConfiguration
{
    public SolutionFiltersConfiguration()
    {
        FilterDefinitions = new List<FilterDefinition>();
        SolutionFile = string.Empty;
    }
    public SolutionFiltersConfiguration(string solutionFile, List<FilterDefinition> filterDefinitions)
    {
        SolutionFile = solutionFile;
        FilterDefinitions = filterDefinitions;
    }

    [Required]
    public string SolutionFile { get; set; }
    
    [Required]
    public List<FilterDefinition> FilterDefinitions { get; set; }

    public static SolutionFiltersConfiguration FromFile(string filterFilePath)
    {
        if (filterFilePath.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase))
        {
            var opts = new JsonSerializerOptions(JsonSerializerOptions.Default)
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<SolutionFiltersConfiguration>(File.ReadAllText(filterFilePath), opts) ??
                throw new Exception($"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file");
        }

        if (!filterFilePath.EndsWith(".yml", StringComparison.CurrentCultureIgnoreCase) &&
            !filterFilePath.EndsWith(".yaml", StringComparison.CurrentCultureIgnoreCase))
            throw new NotSupportedException(
                $"Failed to deserialize {filterFilePath} due to unsupported file type. Only JSON and YAML supported");
        
        var deserializer = new DeserializerBuilder()
            .WithCaseInsensitivePropertyMatching()
            .WithEnforceNullability().WithEnforceRequiredMembers()
            .Build();
        return deserializer.Deserialize<SolutionFiltersConfiguration>(File.ReadAllText(filterFilePath)) ??
               throw new Exception($"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file");
    }
}