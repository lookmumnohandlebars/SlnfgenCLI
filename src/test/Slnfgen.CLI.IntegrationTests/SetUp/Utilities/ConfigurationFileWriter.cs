using Slnfgen.Application.Domain.Filters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Slnfgen.CLI.IntegrationTests.Utilities;

public class ConfigurationFileWriter
{
    private readonly string _directoryPath;

    private static readonly ISerializer Serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public ConfigurationFileWriter(string directoryPath)
    {
        _directoryPath = directoryPath;
    }

    public void Write(string fileName, SolutionFiltersManifest configuration)
    {
        var filePath = Path.Combine(_directoryPath, fileName);
        var yaml = Serializer.Serialize(configuration);
        File.WriteAllText(filePath, yaml);
    }
}
