using Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;
using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Application.Repositories.Solution.Project;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.File.Services;
using Slnfgen.CLI.Domain.Solution.Filter.Services;

namespace Slnfgen.CLI.UnitTests.Domain.Solution.File.Services;

public class SolutionGeneratorTests
{
    private readonly SolutionGenerator _sut = new(new ProjectFileLoader(), new ProjectSuffixFinder()); // An actual file loader is required here

    [Fact]
    public void GenerateMany_should_generate_solutions_for_all_declared_solutions()
    {
        var solutionFile = LoadSolutionFileFromBasicSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromBasicSolution("monorepo.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition);

        res.Count().Should().Be(1);
    }

    [Fact]
    public void GenerateMany_should_generate_solutions_that_include_dependant_projects()
    {
        var solutionFile = LoadSolutionFileFromBasicSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromBasicSolution("monorepo.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition).ToList();
        var filterOne = res.First(filter => filter.Name == "SolutionOne");
        filterOne.ProjectPaths.Should().HaveCount(4);
        filterOne
            .ProjectPaths.Should()
            .BeEquivalentTo(@"ProjA\ProjA.csproj", @"ProjC\ProjC.csproj", @"ProjE\ProjE.csproj", @"ProjF\ProjF.csproj");
    }

    #region SetUp

    private static RootSolutionFile LoadSolutionFileFromBasicSolution(string slnFile)
    {
        return new SolutionFileLoader().Load(Path.Combine("TestSolutions", "BasicSolution", slnFile));
    }

    private static RootSolutionFile LoadSolutionFileFromSolutionWithTests(string slnFile)
    {
        return new SolutionFileLoader().Load(Path.Combine("TestSolutions", "SolutionWithTests", slnFile));
    }

    private static SolutionFiltersManifest LoadManifestFileFromBasicSolution(string manifestFile)
    {
        return new SolutionFiltersManifestFileLoader().Load(
            Path.Combine("TestSolutions", "BasicSolution", manifestFile)
        );
    }

    private static SolutionFiltersManifest LoadManifestFileFromSolutionWithTests(string manifestFile)
    {
        return new SolutionFiltersManifestFileLoader().Load(
            Path.Combine("TestSolutions", "SolutionWithTests", manifestFile)
        );
    }

    #endregion
}
