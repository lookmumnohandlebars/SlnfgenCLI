using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Application.Repositories.Solution.Filter;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.Filter.Models;

namespace Slnfgen.CLI.UnitTests.Application.Repositories.Solution.File;

public class XmlSolutionFileWriterTests : IDisposable
{
    private static readonly string TestDirectory = Path.Combine(Directory.GetCurrentDirectory(), Path.GetTempPath());

    [Fact]
    public void Write_ShouldReturnFilePath_WhenXmlSolutionFileIsValid()
    {
        // Arrange
        var xmlSolutionFile = new XmlSolutionFile(
            "TestSolution",
            new List<string>() { "Project1.csproj", "Project2.csproj" }
        );

        var sut = new XmlSolutionFileWriter();

        // Act
        var filePath = sut.Write(xmlSolutionFile, TestDirectory);

        // Assert
        filePath.Should().NotBeNullOrEmpty();
        filePath.Should().Be(Path.Combine(TestDirectory, "TestSolution.slnx"));
    }

    [Fact]
    public void Write_should_write_xml_solution_file()
    {
        // Arrange
        var xmlSolutionFile = new XmlSolutionFile(
            "TestSolution",
            new List<string>() { "Project1.csproj", "Project2.csproj" }
        );

        var sut = new XmlSolutionFileWriter();

        // Act
        var filePath = sut.Write(xmlSolutionFile, TestDirectory);
        var xml = System.IO.File.ReadAllText(filePath);
        xml.Should().NotBeNullOrEmpty();
        xml.Should()
            .Be(
                """
                <Solution>
                  <Project Path="Project1.csproj" />
                  <Project Path="Project2.csproj" />
                </Solution>
                """
            );
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
