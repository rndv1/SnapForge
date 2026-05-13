namespace SnapForge.Cli.Utils;

public static class PathHelper
{
    public static bool TryResolveFullPath(string path, out string fullPath, out string? error)
    {
        try
        {
            fullPath = Path.GetFullPath(path);
            error = null;

            return true;
        }
        catch (Exception exception) when (exception is ArgumentException or NotSupportedException or PathTooLongException)
        {
            fullPath = string.Empty;
            error = exception.Message;

            return false;
        }
    }

    public static bool PathsEqual(string firstPath, string secondPath)
    {
        var comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        return string.Equals(
            Path.TrimEndingDirectorySeparator(firstPath),
            Path.TrimEndingDirectorySeparator(secondPath),
            comparison);
    }
}
