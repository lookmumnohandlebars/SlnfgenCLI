namespace Slnfgen.CLI.Application.Common.Requests.Validation;

/// <summary>
///
/// </summary>
public class BadRequestException : Exception
{
    /// <inheritdoc />
    public BadRequestException(string reason)
        : base(reason) { }

    /// <inheritdoc />
    public BadRequestException(string reason, Exception inner)
        : base(reason, inner) { }
}
