using System.Text.Json;
using Slnfgen.Application.Features.SolutionFilter;

namespace Slnfgen.CLI.IntegrationTests.Utilities;

public class FilterFileLoader
{
    public SolutionFilter LoadFromFile(string filePath)
    {
        using var fileStream = File.OpenRead(filePath);
        var solutionFilter = JsonSerializer.Deserialize<SolutionFilter>(fileStream);

        if (solutionFilter == null)
        {
            throw new InvalidOperationException($"Failed to deserialize solution filter from file: {filePath}");
        }

        return solutionFilter;
    }

    public IEnumerable<SolutionFilter> LoadDirectory(string directoryPath)
    {
        return Directory.GetFiles(directoryPath, "*.slnf").Select(LoadFromFile);
    }
}
