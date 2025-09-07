using FluentAssertions.Collections;
using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Domain.Solution.Filter.Services;
using Slnfgen.CLI.Domain.Solution.Project.Models;

namespace Slnfgen.CLI.UnitTests.Domain.Solution.SolutionFilter.Services;

public class TestProjectFinderTests
{
    private readonly TestProjectFinder _testProjectFinder;

    public TestProjectFinderTests()
    {
        _testProjectFinder = new TestProjectFinder();
    }

    [Fact]
    public void FindTestProjects_ShouldReturnEmpty_WhenNoPatterns()
    {
        var testProjectPatterns = new HashSet<string> { };
        var slnfile = new SolutionFileLoader().Load(
            Path.Combine("TestSolutions", "SolutionWithTests", "SolutionWithTests.slnx")
        );
        var result = _testProjectFinder.FindTestProjects(slnfile, slnfile.ProjectsInSolution, testProjectPatterns);
        result.ToList().Should().BeEmpty();
    }

    [Fact]
    public void FindTestProjects_ShouldReturnEmpty_WhenNoTestProjectsMatchingPattern()
    {
        var testProjectPatterns = new HashSet<string> { "UnitTests" };
        var slnfile = new SolutionFileLoader().Load(
            Path.Combine("TestSolutions", "SolutionWithTests", "SolutionWithTests.slnx")
        );
        var result = _testProjectFinder
            .FindTestProjects(slnfile, slnfile.ProjectsInSolution, testProjectPatterns)
            .ToList();
        result.Should().BeEmpty();
    }

    [Fact]
    public void FindTestProjects_ShouldReturnTestProject_ForSingleProjectAndTestPattern()
    {
        var testProjectPatterns = new HashSet<string> { "Unit.Tests" };
        var slnfile = new SolutionFileLoader().Load(
            Path.Combine("TestSolutions", "SolutionWithTests", "SolutionWithTests.slnx")
        );
        var result = _testProjectFinder.FindTestProjects(
            slnfile,
            slnfile.ProjectsInSolution.Where(proj => proj.Name == "ProjA"),
            testProjectPatterns
        );
        result.Select(r => r.Name).Should().BeEquivalentTo(new List<string> { "ProjA.Unit.Tests" });
    }

    [Fact]
    public void FindTestProjects_ShouldReturnMatchingTestProjects_ForSingleProjectAndMutipleTestPatterns()
    {
        var testProjectPatterns = new HashSet<string> { "Unit.Tests", "Integration.Tests" };
        var slnfile = new SolutionFileLoader().Load(
            Path.Combine("TestSolutions", "SolutionWithTests", "SolutionWithTests.slnx")
        );
        var result = _testProjectFinder.FindTestProjects(
            slnfile,
            slnfile.ProjectsInSolution.Where(proj => proj.Name == "ProjA"),
            testProjectPatterns
        );
        result
            .Select(r => r.Name)
            .Should()
            .BeEquivalentTo(new List<string> { "ProjA.Unit.Tests", "ProjA.Integration.Tests" });
    }

    [Fact]
    public void FindTestProjects_ShouldReturnMatchingTestProjects_ForMultipleProjectAndMutipleTestPatterns()
    {
        var testProjectPatterns = new HashSet<string> { "Unit.Tests", "Integration.Tests" };
        var slnfile = new SolutionFileLoader().Load(
            Path.Combine("TestSolutions", "SolutionWithTests", "SolutionWithTests.slnx")
        );
        var result = _testProjectFinder.FindTestProjects(slnfile, slnfile.ProjectsInSolution, testProjectPatterns);
        result
            .Select(r => r.Name)
            .Should()
            .BeEquivalentTo(new List<string> { "ProjA.Unit.Tests", "ProjA.Integration.Tests", "ProjB.Unit.Tests" });
    }
}
