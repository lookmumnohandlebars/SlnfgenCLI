using FluentValidation;
using Slnfgen.CLI.Application.Features.SolutionFilter.Requests;

namespace Slnfgen.CLI.Application.Requests.SolutionFilter.Generate;

internal class GenerateSolutionFiltersRequestValidator : AbstractValidator<GenerateSolutionFiltersRequest>
{
    public GenerateSolutionFiltersRequestValidator()
    {
        RuleFor(req => req.OutputDirectory)
            .NotNull()
            .WithMessage("Output directory is required")
            .NotEmpty()
            .WithMessage("Output directory must not be empty")
            .Must(dir => !dir.Any(c => Path.GetInvalidPathChars().Contains(c)))
            .WithMessage("Output directory must be specified as a valid directory");

        RuleFor(req => req.FiltersConfigFilePath)
            .NotNull()
            .WithMessage("Manifest File must be provided")
            .NotEmpty()
            .WithMessage("Manifest File should not be an empty string")
            .Must(File.Exists)
            .WithMessage(
                "Manifest File must exist - please ensure the file path provided is relative to the current directory and matching with the provided input"
            );
    }
}
