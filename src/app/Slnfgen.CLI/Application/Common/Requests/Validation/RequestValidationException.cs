using FluentValidation;
using FluentValidation.Results;

namespace Slnfgen.CLI;

internal class RequestValidationException : ValidationException
{
    private const string DefaultMessage = "Invalid request. Please see the validation errors for more details.";

    public RequestValidationException(IEnumerable<ValidationFailure> errors)
        : base(DefaultMessage, errors) { }
}
