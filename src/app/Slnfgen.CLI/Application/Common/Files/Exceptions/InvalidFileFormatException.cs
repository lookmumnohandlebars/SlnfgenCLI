using Slnfgen.CLI.Application.Common.Requests.Validation;

namespace Slnfgen.CLI.Application.Common.Files.Exceptions;

/// <summary>
///     Exception thrown when a file is invalid or not supported.
/// </summary>
internal class InvalidFileException : BadRequestException
{
    public InvalidFileException(string message)
        : base(message) { }

    public InvalidFileException(string message, Exception inner)
        : base(message, inner) { }
}
