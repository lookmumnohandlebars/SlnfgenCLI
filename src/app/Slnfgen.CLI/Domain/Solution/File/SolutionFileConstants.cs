namespace Slnfgen.CLI.Domain.Solution.File;

/// <summary>
///     Constants for solution file operations.
/// </summary>
public static class SolutionFileConstants
{
    /// <summary>
    ///     Standard file extension for solution files.
    /// </summary>
    public const string FileExtension = ".sln";

    /// <summary>
    ///     Alternative file extension for solution files, used for XML-based solutions (introduced in .NET 8).
    /// </summary>
    public const string XmlFileExtension = ".slnx";

    /// <summary>
    ///     All supported file extensions for solution files.
    /// </summary>
    public static readonly string[] SupportedFileExtensions = [FileExtension, XmlFileExtension];
}
