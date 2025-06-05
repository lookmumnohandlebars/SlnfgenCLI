using Microsoft.Build.Construction;
using Slnfgen.Application.Features.Solution;

namespace Slnfgen.CLI.UnitTests.Domain.Solution;

public class RootSolutionFileTests
{
    [Fact]
    public void FromSolutionFilePath_should_contain_parse_from_input_path_to_slnx_file()
    {
        var solutionFile = RootSolutionFile.FromSolutionFilePath(Path.Combine("TestSolution", "TestSolution.slnx"));
        solutionFile.ProjectsInSolution.Should().HaveCount(7);
        solutionFile.Path.Should().Be(Path.Combine(Directory.GetCurrentDirectory(),"TestSolution", "TestSolution.slnx"));
    }
    
    [Fact]
    public void FromSolutionFilePath_should_contain_parse_from_input_path_to_sln_file()
    {
        var solutionFile = RootSolutionFile.FromSolutionFilePath(Path.Combine("TestSolution", "TestSolutionLegacy.sln"));
        Path.Combine(Directory.GetCurrentDirectory(), "TestSolution", "TestSolution.slnx");
        solutionFile.ProjectsInSolution.Should().HaveCount(6);
        solutionFile.Path.Should().Be(Path.Combine(Directory.GetCurrentDirectory(),"TestSolution", "TestSolutionLegacy.sln"));
    }
    
}