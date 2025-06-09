using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.Solution;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Domain.Solution.File.Loader;

namespace Slnfgen.CLI.UnitTests.Domain.Filters;

public class SolutionFilterGeneratorTests
{
    private readonly SolutionFilterGenerator _sut;

    public SolutionFilterGeneratorTests()
    {
        _sut = new SolutionFilterGenerator();
    }

    [Fact]
    public void GenerateMany_should_generate_filters_for_all_declared_filters()
    {
        var solutionFile = LoadSolutionFileFromTestSolution("TestSolutionLegacy.sln");
        var filtersDefinition = new SolutionFiltersManifestFileLoader().Load(
            Path.Combine("TestSolution", "monorepoLegacy.yml")
        );
        var res = _sut.GenerateMany(solutionFile, filtersDefinition);

        res.Count().Should().Be(2);
    }

    [Fact]
    public void GenerateMany_should_generate_filters_that_include_dependant_projects()
    {
        var solutionFile = LoadSolutionFileFromTestSolution("TestSolutionLegacy.sln");
        var filtersDefinition = new SolutionFiltersManifestFileLoader().Load(
            Path.Combine("TestSolution", "monorepoLegacy.yml")
        );
        var res = _sut.GenerateMany(solutionFile, filtersDefinition).ToList();
        var filterOne = res.First(filter => filter.Name == "FilterOne");
        filterOne.Solution.Path.Should().Be("TestSolutionLegacy.sln");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(["ProjA/ProjA.csproj", "ProjC/ProjC.csproj", "ProjE/ProjE.csproj", "ProjF/ProjF.csproj"]);

        res.Where(filter => filter.Name == "FilterTwo")
            .SelectMany(filter => filter.Solution.Projects)
            .Should()
            .BeEquivalentTo(["Projb/ProjB.csproj", "ProjD/ProjD.csproj", "ProjF/ProjF.csproj", "ProjG/ProjG.csproj"]);
    }

    #region SetUp

    private static RootSolutionFile LoadSolutionFileFromTestSolution(string slnFile) =>
        new SolutionFileLoader().Load(Path.Combine("TestSolution", slnFile));

    #endregion
}
