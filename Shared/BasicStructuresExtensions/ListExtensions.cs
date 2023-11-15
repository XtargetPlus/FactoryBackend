namespace Shared.BasicStructuresExtensions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
    {
        if (source is null)
            return true;
        return !source.Any();
    }
}
