namespace Slnfgen.CLI.Common.Paths;

/// <summary>
///     Extensions for path normalization and manipulation.
///     All paths should be normalized to use backslashes (\) as directory separators
///     This is to ensure consistency across different operating systems and environments.
///     Backslashes are the standard directory separator on Windows, which is the primary target platform for
///     .NET development.
/// </summary>
public static class PathUtilities
{
    /// <summary>
    ///     Normalizes a path to use backslashes (\) as directory separators.
    ///     This method replaces all forward slashes (/) with backslashes (\).
    ///     It also ensures that any single backslashes are converted to double backslashes (\\) to avoid issues with escape characters in strings.
    ///     If the input path is null or empty, it returns the input as is.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string NormalizePathToBackslashes(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;
        return path.Replace("/", "\\");
    }
}
