using System.Text.Json;
using YamlDotNet.Serialization;

namespace Slnfgen.Application.Domain.Filters;

/// <summary>
///
/// </summary>
public class SolutionFiltersManifestFileLoader : ISolutionFiltersManifestLoader
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="filterFilePath"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public SolutionFiltersManifest Load(string filterFilePath)
    {
        if (filterFilePath.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase))
        {
            var opts = new JsonSerializerOptions(JsonSerializerOptions.Default) { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<SolutionFiltersManifest>(File.ReadAllText(filterFilePath), opts)
                ?? throw new Exception(
                    $"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file"
                );
        }

        if (
            !filterFilePath.EndsWith(".yml", StringComparison.CurrentCultureIgnoreCase)
            && !filterFilePath.EndsWith(".yaml", StringComparison.CurrentCultureIgnoreCase)
        )
            throw new NotSupportedException(
                $"Failed to deserialize {filterFilePath} due to unsupported file type. Only JSON and YAML supported"
            );

        var deserializer = new DeserializerBuilder()
            .WithCaseInsensitivePropertyMatching()
            .WithEnforceNullability()
            .WithEnforceRequiredMembers()
            .Build();

        return deserializer.Deserialize<SolutionFiltersManifest>(File.ReadAllText(filterFilePath))
            ?? throw new Exception(
                $"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file"
            );
    }
}
