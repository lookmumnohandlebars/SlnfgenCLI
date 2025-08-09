namespace Slnfgen.CLI;

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
