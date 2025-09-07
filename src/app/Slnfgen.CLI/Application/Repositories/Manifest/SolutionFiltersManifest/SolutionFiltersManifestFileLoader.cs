using System.Text.Json;
using Slnfgen.CLI.Application.Common.Files.Exceptions;
using Slnfgen.CLI.Application.Common.Requests.Validation;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Repository;
using YamlDotNet.Serialization;

namespace Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;

/// <summary>
///     Loads a manifest file for generating solution filters
/// </summary>
public class SolutionFiltersManifestFileLoader : ISolutionFiltersManifestLoader
{
    /// <summary>
    ///     Loads a JSON or YAML manifest file for generating solution filters.
    /// </summary>
    /// <param name="filterFilePath">The relative path to the manifest file</param>
    /// <returns>The loaded manifest file</returns>
    /// <exception cref="BadRequestException">Thrown when the file does not exist</exception>
    /// <exception cref="NotSupportedException">Thrown when the file type is not supported</exception>
    /// <exception cref="InvalidFileException">Thrown when the file cannot be deserialized</exception>
    /// <exception cref="Exception">Thrown when the file cannot be deserialized due to formatting issues</exception>
    public Domain.Manifest.SolutionFiltersManifest.Models.SolutionFiltersManifest Load(string filterFilePath)
    {
        var normalizedPath = Path.GetFullPath(filterFilePath);
        if (!File.Exists(normalizedPath))
            throw new BadRequestException(
                $"The file {filterFilePath} does not exist. Please check the path and try again."
            );

        if (filterFilePath.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase))
        {
            var opts = new JsonSerializerOptions(JsonSerializerOptions.Default) { PropertyNameCaseInsensitive = true };
            var manifestFromJson =
                JsonSerializer.Deserialize<Domain.Manifest.SolutionFiltersManifest.Models.SolutionFiltersManifest>(
                    File.ReadAllBytes(normalizedPath),
                    opts
                )
                ?? throw new Exception(
                    $"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file"
                );
            return new Domain.Manifest.SolutionFiltersManifest.Models.SolutionFiltersManifest(
                manifestFromJson.SolutionFile,
                manifestFromJson.FilterDefinitions,
                manifestFromJson.TestProjectPatterns
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
                deserializer.Deserialize<Domain.Manifest.SolutionFiltersManifest.Models.SolutionFiltersManifest>(
                    File.ReadAllText(filterFilePath)
                )
                ?? throw new Exception(
                    $"Failed to deserialize {filterFilePath}. Please check the formatting and path of the file"
                );
            // Hack to ensure the constructor is called with the correct parameters
            return new Domain.Manifest.SolutionFiltersManifest.Models.SolutionFiltersManifest(
                manifestFromYaml.SolutionFile,
                manifestFromYaml.FilterDefinitions,
                manifestFromYaml.TestProjectPatterns
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
