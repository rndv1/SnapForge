namespace SnapForge.Core.Utils;

public static class ImageFormatResolver
{
    public static bool IsPngPath(string path)
    {
        return string.Equals(Path.GetExtension(path), ".png", StringComparison.OrdinalIgnoreCase);
    }
}
