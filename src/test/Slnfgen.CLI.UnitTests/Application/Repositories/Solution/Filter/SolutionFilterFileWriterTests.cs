using Slnfgen.CLI.Application.Repositories.Solution.Filter;
using Slnfgen.CLI.Domain.Solution.Filter.Models;

namespace Slnfgen.CLI.UnitTests.Application.Repositories.Solution.Filter;

public class SolutionFilterFileWriterTests : IDisposable
{
    private readonly SolutionFilterFileWriter _solutionFilterFileWriter = new();

    private static readonly string TestDirectory = Path.Combine(Directory.GetCurrentDirectory(), Path.GetTempPath());

    [Fact]
    public void Write_ShouldReturnFilePath_WhenSolutionFilterIsValid()
    {
        // Arrange
        var solutionFilter = new SolutionFilter(
            "TestFilter",
            new SolutionFiltersSolutionDefinition("MySolution.sln", new string[] { "Myproj.csproj" })
        );

        // Act
        var filePath = _solutionFilterFileWriter.Write(solutionFilter, TestDirectory);

        // Assert
        filePath.Should().NotBeNullOrEmpty();
        filePath.Should().Be(Path.Combine(TestDirectory, "TestFilter.slnf"));
    }

    [Fact]
    public void Write_should_write_solution_filter()
    {
        // Arrange
        var solutionFilter = new SolutionFilter(
            "TestFilter",
            new SolutionFiltersSolutionDefinition("MySolution.sln", new string[] { "Myproj.csproj" })
        );

        // Act
        var filePath = _solutionFilterFileWriter.Write(solutionFilter, TestDirectory);

        // Assert
        var loader = new SolutionFilterFileLoader();
        var writtenSolutionFilter = loader.LoadOne(filePath);
        writtenSolutionFilter.Should().BeEquivalentTo(solutionFilter);
    }

    public void Dispose()
    {
        // Clean up test files
        var files = Directory.GetFiles(TestDirectory);
        foreach (var file in files)
        {
            try
            {
                System.IO.File.Delete(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete file {file}: {ex.Message}");
            }
        }
    }
}
