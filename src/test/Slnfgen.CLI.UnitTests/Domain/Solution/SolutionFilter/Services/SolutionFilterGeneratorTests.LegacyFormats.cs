namespace Slnfgen.CLI.UnitTests.Domain.Filters;

public partial class SolutionFilterGeneratorTests
{
    [Fact]
    public void GenerateMany_should_generate_filters_for_all_declared_filters_with_legacy_formats()
    {
        var outputDirectory = "TestSolution";
        var solutionFile = LoadSolutionFileFromTestSolution("TestSolutionLegacy.sln");
        var filtersDefinition = LoadManifestFileFromTestSolution("monorepoLegacy.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition, outputDirectory);

        res.Count().Should().Be(2);
    }

    [Fact]
    public void GenerateMany_should_generate_filters_that_include_dependant_projects_with_legacy_formats()
    {
        var outputDirectory = "TestSolution";
        var solutionFile = LoadSolutionFileFromTestSolution("TestSolutionLegacy.sln");
        var filtersDefinition = LoadManifestFileFromTestSolution("monorepoLegacy.yml");
        var res = _sut.GenerateMany(solutionFile, filtersDefinition, outputDirectory).ToList();
        var filterOne = res.First(filter => filter.Name == "FilterOne");
        filterOne.Solution.Path.Should().Be("TestSolutionLegacy.sln");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj"
            );

        res.Where(filter => filter.Name == "FilterTwo")
            .SelectMany(filter => filter.Solution.Projects)
            .Should()
            .BeEquivalentTo(
                @"Projb\\Nested\\ProjB.csproj",
                @"ProjD\\ProjD.csproj",
                @"ProjF\\ProjF.csproj"
            // ProjG not in Legacy manifest, so not included
            );
    }
}
