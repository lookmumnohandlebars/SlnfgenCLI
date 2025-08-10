using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Slnfgen.CLI.IntegrationTests.SetUp.Utilities;

public class ManifestFileWriter
{
    private static readonly ISerializer Serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    private readonly string _directoryPath;

    public ManifestFileWriter(string directoryPath)
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
