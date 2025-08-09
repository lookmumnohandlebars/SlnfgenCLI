using System.Text.Json;
using FluentValidation;
using Slnfgen.Application.Module.Common.Files.Exceptions;
using Slnfgen.CLI;
using YamlDotNet.Serialization;

namespace Slnfgen.Application.Domain.Filters;

/// <summary>
/// </summary>
public class SolutionFiltersManifestFileLoader : ISolutionFiltersManifestLoader
{
    /// <summary>
    /// </summary>
    /// <param name="filterFilePath"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public SolutionFiltersManifest Load(string filterFilePath)
    {
        var normalizedPath = Path.GetFullPath(filterFilePath);
        if (!File.Exists(normalizedPath))
            throw new BadRequestException(
                $"The file {filterFilePath} does not exist. Please check the path and try again."
            );

        if (filterFilePath.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase))
        {
            var opts = new JsonSerializerOptions(JsonSerializerOptions.Default)
            {
                PropertyNameCaseInsensitive = true,
            };
            var manifestFromJson =
                JsonSerializer.Deserialize<SolutionFiltersManifest>(
                    File.ReadAllBytes(normalizedPath),
                    opts
                )
                ?? throw new Exception(
                    $"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file"
                );
            return new SolutionFiltersManifest(
                manifestFromJson.SolutionFile,
                manifestFromJson.FilterDefinitions
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

        try
        {
            var manifestFromYaml =
                deserializer.Deserialize<SolutionFiltersManifest>(File.ReadAllText(filterFilePath))
                ?? throw new Exception(
                    $"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file"
                );
            // Hack to ensure the constructor is called with the correct parameters
            return new SolutionFiltersManifest(
                manifestFromYaml.SolutionFile,
                manifestFromYaml.FilterDefinitions
            );
        }
        catch (YamlDotNet.Core.YamlException e)
        {
            throw new InvalidFileException(
                $"Failed to deserialize {filterFilePath} due to YAML parsing error: {e.Message}",
                e
            );
        }
    }
}
