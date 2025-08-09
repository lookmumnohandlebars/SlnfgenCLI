using Microsoft.Extensions.Logging.Testing;
using Slnfgen.Application.Domain.Filters;
using Slnfgen.Application.Features.SolutionFilterGeneration;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;
using Slnfgen.CLI.Application.Repository.Solution.Project;
using Slnfgen.CLI.Application.Requests.GenerateOne;
using Slnfgen.CLI.Application.Services.SolutionFilter;
using Slnfgen.CLI.Domain.Solution.File.Loader;
using Slnfgen.CLI.TestImplementations.Application.Repository;

namespace Slnfgen.CLI.UnitTests.Application.Requests.SolutionFilter.Generate;

public class GenerateOneSolutionFilterRequestHandlerTests
{
    private GenerateSolutionFilterRequestHandler _sut;
    private FakeSolutionFilterWriter _fakeSolutionFilterWriter = new();

    public GenerateOneSolutionFilterRequestHandlerTests()
    {
        _sut = new GenerateSolutionFilterRequestHandler(
            new SolutionFilterGenerator(new ProjectFileLoader()),
            _fakeSolutionFilterWriter,
            new SolutionFiltersManifestFileLoader(),
            new SolutionFileLoader(),
            new FakeLogger<GenerateSolutionFiltersRequestHandler>()
        );
    }

    [Fact]
    public void Handle_ShouldThrowValidationErrorIfFilterDoesntMatch()
    {
        // Arrange
        var request = new GenerateSolutionFilterRequest(
            Path.Combine("TestSolution", "monorepo.yml"),
            "NotAFilter",
            "."
        );

        // Act
        var act = () => _sut.Handle(request);
        act.Should().Throw<BadRequestException>();
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters()
    {
        // Arrange
        var request = new GenerateSolutionFilterRequest(Path.Combine("TestSolution", "monorepo.yml"), "FilterOne", ".");

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        var generatedFilterPath = Path.Combine(".", "FilterOne");
        response.GeneratedFilter.Should().Be(generatedFilterPath);
        var filterOne = _fakeSolutionFilterWriter.Store[generatedFilterPath];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolution\\TestSolution.slnx");
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
    public void Handle_ShouldNotGenerateSolutionFiltersForNonTargetFilterDefinitions()
    {
        // Arrange
        var request = new GenerateSolutionFilterRequest(Path.Combine("TestSolution", "monorepo.yml"), "FilterOne", ".");

        // Act
        var response = _sut.Handle(request);

        // Assert
        _fakeSolutionFilterWriter.Store.Should().HaveCount(1);
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters_ForLegacySolutionFormat()
    {
        // Arrange
        var request = new GenerateSolutionFilterRequest(
            Path.Combine("TestSolution", "monorepoLegacy.yml"),
            "FilterTwo",
            "."
        );

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        var generatedFilterPath = Path.Combine(".", "FilterTwo");
        response.GeneratedFilter.Should().Be(generatedFilterPath);
        var filterOne = _fakeSolutionFilterWriter.Store[generatedFilterPath];
        filterOne.Should().NotBeNull();
        filterOne.Solution.Path.Should().Be(@"TestSolution\\TestSolutionLegacy.sln");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(@"Projb\\Nested\\Projb.csproj", @"ProjD\\ProjD.csproj", @"ProjF\\ProjF.csproj");
    }
}
