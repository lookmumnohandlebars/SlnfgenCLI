using Slnfgen.CLI.Domain.Solution.Project;

namespace Slnfgen.CLI.UnitTests.Domain.Solution.Project;

public class ProjectFileTests
{
    [Fact]
    public void LoadFromFile_should_successfully_load_from_absolute_file_path()
    {
        var proj = ProjectFile.LoadFromFile(Path.Combine(Directory.GetCurrentDirectory(), "TestSolution", "ProjA", "ProjA.csproj"));
        proj.FullPath.Should().Be(Path.Combine(Directory.GetCurrentDirectory(), "TestSolution", "ProjA", "ProjA.csproj"));
        proj.ProjectDependencies.Should().HaveCount(2);
    }
    
    [Fact]
    public void LoadFromFile_should_successfully_load_from_relative_file()
    {
        var proj = ProjectFile.LoadFromFile(Path.Combine("TestSolution", "ProjA", "ProjA.csproj"));
        proj.FullPath.Should().Be(Path.Combine(Directory.GetCurrentDirectory(), "TestSolution", "ProjA", "ProjA.csproj"));
        proj.ProjectDependencies.Should().HaveCount(2);
    }
}