using Slnfgen.CLI.Application.Repositories.Solution.File;

namespace Slnfgen.CLI.UnitTests.Application.Repositories.Solution.File;

public class SolutionFileLoaderTests
{
    private readonly SolutionFileLoader _sut;

    public SolutionFileLoaderTests()
    {
        _sut = new SolutionFileLoader();
    }

    [Fact]
    public void Load_should_contain_parse_from_input_path_to_slnx_file()
    {
        var location = Path.Combine("TestSolutions", "BasicSolution", "TestSolution.slnx");
        var solutionFile = _sut.Load(location);
        var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), location);
        solutionFile.ProjectsInSolution.Should().HaveCount(7);
        solutionFile.AbsolutePath.Should().Be(absolutePath);
    }

    [Fact]
    public void Load_should_contain_parse_from_input_path_to_sln_file()
    {
        var location = Path.Combine("TestSolutions", "BasicSolution", "TestSolutionLegacy.sln");
        var solutionFile = _sut.Load(location);
        var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), location);
        solutionFile.ProjectsInSolution.Should().HaveCount(6);
        solutionFile.AbsolutePath.Should().Be(absolutePath);
    }

    [Fact]
    public void Load_should_throw_clear_exception_if_not_parsing_solution_file()
    {
        var location = Path.Combine("TestSolutions", "BasicSolution", "monorepo.yml");
        var act = () => _sut.Load(location);
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"Invalid solution file path: {location}. Must end with .sln or slnx");
    }
}
