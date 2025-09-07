using Microsoft.Extensions.Logging.Testing;
using Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;
using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Application.Repositories.Solution.Project;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Slnfgen.CLI.Domain.Solution.Filter.Services;
using Slnfgen.CLI.UnitTests.Application.TestImplementations;

namespace Slnfgen.CLI.UnitTests.Application.Requests.Generate;

public class GenerateSolutionFiltersRequestHandlerTests
{
    private GenerateSolutionFiltersRequestHandler _sut;
    private FakeSolutionFilterWriter _fakeSolutionFilterWriter = new();

    public GenerateSolutionFiltersRequestHandlerTests()
    {
        _sut = new GenerateSolutionFiltersRequestHandler(
            new SolutionFilterGenerator(new ProjectFileLoader(), new TestProjectFinder()),
            _fakeSolutionFilterWriter,
            new SolutionFiltersManifestFileLoader(),
            new SolutionFileLoader(),
            new FakeLogger<GenerateSolutionFiltersRequestHandler>()
        );
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters()
    {
        // Arrange
        var request = new GenerateSolutionFiltersRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepo.yml"),
            "."
        );

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        response.GeneratedFilters.Should().HaveCount(2);

        var filterOne = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterOne")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolutions\\BasicSolution\\TestSolution.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj",
                @"ProjG\\ProjG.csproj"
            );

        var filterTwo = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterTwo")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolutions\\BasicSolution\\TestSolution.slnx");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj",
                @"ProjG\\ProjG.csproj"
            );
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters_ForLegacySolutionFormat()
    {
        // Arrange
        var request = new GenerateSolutionFiltersRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepoLegacy.yml"),
            "."
        );

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        response.GeneratedFilters.Should().HaveCount(2);
        var filterOne = _fakeSolutionFilterWriter.Store[Path.Combine(".", "FilterOne")];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolutions\\BasicSolution\\TestSolutionLegacy.sln");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(
                @"ProjA\\ProjA.csproj",
                @"ProjC\\ProjC.csproj",
                @"ProjE\\ProjE.csproj",
                @"ProjF\\ProjF.csproj"
            );
    }
}
