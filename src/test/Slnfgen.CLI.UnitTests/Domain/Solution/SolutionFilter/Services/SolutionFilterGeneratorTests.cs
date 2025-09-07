using Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;
using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Application.Repositories.Solution.Project;
using Slnfgen.CLI.Domain.Manifest.SolutionFiltersManifest.Models;
using Slnfgen.CLI.Domain.Solution.File.Models;
using Slnfgen.CLI.Domain.Solution.Filter.Services;

namespace Slnfgen.CLI.UnitTests.Domain.Solution.SolutionFilter.Services;

public partial class SolutionFilterGeneratorTests
{
    private readonly SolutionFilterGenerator _sut = new(new ProjectFileLoader(), new TestProjectFinder()); // An actual file loader is required here

    [Fact]
    public void GenerateMany_should_generate_filters_for_all_declared_filters()
    {
        var outputDirectory = Path.Combine("TestSolutions", "BasicSolution");
        var solutionFile = LoadSolutionFileFromBasicSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromBasicSolution("monorepo.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition, outputDirectory);

        res.Count().Should().Be(2);
    }

    [Fact]
    public void GenerateMany_should_generate_filters_that_include_dependant_projects()
    {
        var outputDirectory = Path.Combine("TestSolutions", "BasicSolution");
        var solutionFile = LoadSolutionFileFromBasicSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromBasicSolution("monorepo.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition, outputDirectory).ToList();
        var filterOne = res.First(filter => filter.Name == "FilterOne");
        filterOne.Solution.Path.Should().Be("TestSolution.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj",
                @"ProjG\\ProjG.csproj"
            );

        var filterTwo = res.First(filter => filter.Name == "FilterTwo");
        filterTwo.Solution.Path.Should().Be("TestSolution.slnx");
        filterTwo
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"Projb\\Nested\\Projb.csproj",
                @"ProjD\\ProjD.csproj",
                @"ProjF\\ProjF.csproj",
                @"ProjG\\ProjG.csproj"
            );
    }

    [Fact]
    public void GenerateMany_should_generate_filters_in_sub_directories_with_relative_path_to_solution()
    {
        var outputDirectory = Path.Combine("TestSolutions", "BasicSolution", "SubService");
        var solutionFile = LoadSolutionFileFromBasicSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromBasicSolution("monorepo.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition, outputDirectory).ToList();
        var filterOne = res.First(filter => filter.Name == "FilterOne");
        filterOne.Solution.Path.Should().Be("..\\\\TestSolution.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                "ProjA\\\\ProjA.csproj",
                "ProjC\\\\ProjC.csproj",
                "ProjE\\\\ProjE.csproj",
                "ProjF\\\\ProjF.csproj",
                "ProjG\\\\ProjG.csproj"
            );

        var filterTwo = res.First(filter => filter.Name == "FilterTwo");
        filterTwo.Solution.Path.Should().Be("..\\\\TestSolution.slnx");
        filterTwo
            .Solution.Projects.Should()
            .BeEquivalentTo(
                "Projb\\\\Nested\\\\Projb.csproj",
                "ProjD\\\\ProjD.csproj",
                "ProjF\\\\ProjF.csproj",
                "ProjG\\\\ProjG.csproj"
            );
    }

    [Fact]
    public void GenerateMany_should_generate_filters_that_include_matching_test_dependencies()
    {
        var outputDirectory = Path.Combine("TestSolutions", "SolutionWithTests");
        var solutionFile = LoadSolutionFileFromSolutionWithTests("SolutionWithTests.slnx");
        var filtersDefinition = LoadManifestFileFromSolutionWithTests("monorepo.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition, outputDirectory).ToList();
        var filterOne = res.First(filter => filter.Name == "FilterOne");
        filterOne.Solution.Path.Should().Be("SolutionWithTests.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjA.Unit.Tests\\ProjA.Unit.Tests.csproj",
                @"ProjA.Integration.Tests\\ProjA.Integration.Tests.csproj",
                @"ProjB\\ProjB.csproj",
                @"ProjB.Unit.Tests\\ProjB.Unit.Tests.csproj",
                @"TestUtils\\TestUtils.csproj"
            );

        var filterTwo = res.First(filter => filter.Name == "FilterTwo");
        filterTwo.Solution.Path.Should().Be("SolutionWithTests.slnx");
        filterTwo
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjB\\ProjB.csproj",
                @"ProjB.Unit.Tests\\ProjB.Unit.Tests.csproj",
                @"ProjB.Contract.Tests\\ProjB.Contract.Tests.csproj",
                @"TestUtils\\TestUtils.csproj"
            );
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
