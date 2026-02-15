namespace CodeNet.ApiHost.Extensions;

internal static class Helper
{
    public static string[] SemicolonSplit(this string value) => value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
