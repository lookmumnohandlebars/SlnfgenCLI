using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Slnfgen.CLI;

namespace Slnfgen.Application.Module.Common.Files.Exceptions;

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
