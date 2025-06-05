using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.Solution;
using Slnfgen.Application.Features.SolutionFilterGeneration;

namespace Slnfgen.CLI.UnitTests.Domain.Filters;

public class SolutionFilterGeneratorTests
{
    private readonly SolutionFilterGenerator _sut;

    public SolutionFilterGeneratorTests()
    {
        _sut = new SolutionFilterGenerator();
    }

    [Fact]
    public void Should_generate_filters_for_all_solutions()
    {
        var solutionFile = RootSolutionFile.FromSolutionFilePath(Path.Combine("TestSolution", "TestSolutionLegacy.sln"));
        var filtersDefinition = SolutionFiltersConfiguration.FromFile(Path.Combine("TestSolution", "monorepoLegacy.yml"));
        var res = _sut.GenerateMany(solutionFile, filtersDefinition);
        res.Count().Should().Be(2);
    }

    [Fact]
    public void Should_generate_filters_for_all_projects()
    {
        
    }
}