using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.Solution;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Repository.Solution.Project;
using Slnfgen.CLI.Domain.Solution.File.Loader;

namespace Slnfgen.CLI.UnitTests.Domain.Filters;

public partial class SolutionFilterGeneratorTests
{
    private readonly SolutionFilterGenerator _sut = new(new ProjectFileLoader()); // An actual file loader is required here

    [Fact]
    public void GenerateMany_should_generate_filters_for_all_declared_filters()
    {
        var outputDirectory = "TestSolution";
        var solutionFile = LoadSolutionFileFromTestSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromTestSolution("monorepo.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition, outputDirectory);

        res.Count().Should().Be(2);
    }

    [Fact]
    public void GenerateMany_should_generate_filters_that_include_dependant_projects()
    {
        var outputDirectory = "TestSolution";
        var solutionFile = LoadSolutionFileFromTestSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromTestSolution("monorepo.yml");
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
                @"Projb\\Nested\\ProjB.csproj",
                @"ProjD\\ProjD.csproj",
                @"ProjF\\ProjF.csproj",
                @"ProjG\\ProjG.csproj"
            );
    }

    [Fact]
    public void GenerateMany_should_generate_filters_in_sub_directories_with_relative_path_to_solution()
    {
        var outputDirectory = Path.Combine("TestSolution", "SubService");
        var solutionFile = LoadSolutionFileFromTestSolution("TestSolution.slnx");
        var filtersDefinition = LoadManifestFileFromTestSolution("monorepo.yml");
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
                "Projb\\\\Nested\\\\ProjB.csproj",
                "ProjD\\\\ProjD.csproj",
                "ProjF\\\\ProjF.csproj",
                "ProjG\\\\ProjG.csproj"
            );
    }

    #region SetUp

    private static RootSolutionFile LoadSolutionFileFromTestSolution(string slnFile)
    {
        return new SolutionFileLoader().Load(Path.Combine("TestSolution", slnFile));
    }

    private static SolutionFiltersManifest LoadManifestFileFromTestSolution(string manifestFile)
    {
        return new SolutionFiltersManifestFileLoader().Load(Path.Combine("TestSolution", manifestFile));
    }

    #endregion
}
