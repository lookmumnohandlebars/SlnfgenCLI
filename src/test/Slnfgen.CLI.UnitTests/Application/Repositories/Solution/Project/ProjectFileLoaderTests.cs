using Microsoft.Build.Exceptions;
using Slnfgen.CLI.Application.Repository.Solution.Project;

namespace Slnfgen.CLI.UnitTests.Domain.Solution.Project;

public class ProjectFileLoaderTests
{
    private readonly ProjectFileLoader _sut;

    public ProjectFileLoaderTests()
    {
        _sut = new ProjectFileLoader();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void LoadOne_should_not_throw_with_valid_file_path(bool useAbsolute)
    {
        var path = Path.Combine(
            useAbsolute ? Directory.GetCurrentDirectory() : String.Empty,
            "TestSolution",
            "ProjA",
            "ProjA.csproj"
        );
        var act = () => _sut.LoadOne(path);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void LoadOne_should_load_with_correct_properties_from_absolute_project_file(bool useAbsolute)
    {
        var path = Path.Combine(
            useAbsolute ? Directory.GetCurrentDirectory() : String.Empty,
            "TestSolution",
            "ProjA",
            "ProjA.csproj"
        );
        var proj = _sut.LoadOne(path);
        proj.FullPath.Should().Be(useAbsolute ? path : Path.Combine(Directory.GetCurrentDirectory(), path));
        proj.ProjectDependencies.Should()
            .HaveCount(2)
            .And.BeEquivalentTo("..\\ProjC\\ProjC.csproj", "..\\ProjE\\ProjE.csproj");
    }

    [Theory]
    [InlineData("")]
    [InlineData("project.json")]
    [InlineData("projectMissingSuffix")]
    public void LoadOne_should_throw_if_not_a_csproj(string invalidFilePath)
    {
        var act = () => _sut.LoadOne(invalidFilePath);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void LoadOne_should_throw_InvalidProjectFileException_if_csproj_doesnt_exist()
    {
        var act = () => _sut.LoadOne(Path.Combine("TestSolution", "ProjA", "ProjZ.csproj"));
        act.Should().Throw<InvalidProjectFileException>();
    }

    [Fact]
    public void LoadFromDirectory_should_load_all_project_files_in_test_solution()
    {
        var projectFiles = _sut.LoadFromDirectory("TestSolution").ToList();
        projectFiles.Should().HaveCount(7);

        var projCFile = projectFiles.Single(file => file.FullPath.Contains("ProjC.csproj"));
        projCFile
            .FullPath.Should()
            .Be(Path.Combine(Directory.GetCurrentDirectory(), "TestSolution", "ProjC", "ProjC.csproj"));

        projCFile
            .FullPath.Should()
            .Be(Path.Combine(Directory.GetCurrentDirectory(), "TestSolution", "ProjC", "ProjC.csproj"));
    }
}
