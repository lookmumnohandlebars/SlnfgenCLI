using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace Slnfgen.Application.Module.Common.Files.Exceptions;

/// <summary>
///     Exception thrown when a file is invalid or not supported.
/// </summary>
internal class InvalidFileException : ValidationException
{
    public InvalidFileException(string message)
        : base(message) { }
}
