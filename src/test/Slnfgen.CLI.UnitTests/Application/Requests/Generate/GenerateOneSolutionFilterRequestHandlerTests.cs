using Microsoft.Extensions.Logging.Testing;
using Slnfgen.CLI.Application.Common.Requests.Validation;
using Slnfgen.CLI.Application.Repositories.Manifest.SolutionFiltersManifest;
using Slnfgen.CLI.Application.Repositories.Solution.File;
using Slnfgen.CLI.Application.Repositories.Solution.Project;
using Slnfgen.CLI.Application.Requests.GenerateAll;
using Slnfgen.CLI.Application.Requests.GenerateTarget;
using Slnfgen.CLI.Domain.Solution.Filter.Services;
using Slnfgen.CLI.UnitTests.Application.TestImplementations;

namespace Slnfgen.CLI.UnitTests.Application.Requests.Generate;

public class GenerateOneSolutionFilterRequestHandlerTests
{
    private GenerateTargetSolutionFilterRequestHandler _sut;
    private FakeSolutionFilterWriter _fakeSolutionFilterWriter = new();

    public GenerateOneSolutionFilterRequestHandlerTests()
    {
        _sut = new GenerateTargetSolutionFilterRequestHandler(
            new SolutionFilterGenerator(new ProjectFileLoader(), new TestProjectFinder()),
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
        var request = new GenerateTargetSolutionFilterRequest(
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
        var request = new GenerateTargetSolutionFilterRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepo.yml"),
            "FilterOne",
            "."
        );

        // Act
        var response = _sut.Handle(request);

        // Assert
        response.Should().NotBeNull();
        var generatedFilterPath = Path.Combine(".", "FilterOne");
        response.GeneratedFilter.Should().Be(generatedFilterPath);
        var filterOne = _fakeSolutionFilterWriter.Store[generatedFilterPath];
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
    public void Handle_ShouldNotGenerateSolutionFiltersForNonTargetFilterDefinitions()
    {
        // Arrange
        var request = new GenerateTargetSolutionFilterRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepo.yml"),
            "FilterOne",
            "."
        );

        // Act
        _sut.Handle(request);

        // Assert
        _fakeSolutionFilterWriter.Store.Should().HaveCount(1);
    }

    [Fact]
    public void Handle_ShouldGenerateSolutionFilters_ForLegacySolutionFormat()
    {
        // Arrange
        var request = new GenerateTargetSolutionFilterRequest(
            Path.Combine("TestSolutions", "BasicSolution", "monorepoLegacy.yml"),
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
        filterOne.Solution.Path.Should().Be(@"TestSolutions\\BasicSolution\\TestSolutionLegacy.sln");
        filterOne
            .Solution.Projects.Should()
            .BeEquivalentTo(@"Projb\\Nested\\Projb.csproj", @"ProjD\\ProjD.csproj", @"ProjF\\ProjF.csproj");
    }
}
