namespace Sampling.UnitTests.TestExtensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items) => items.OrderBy(_ => Guid.NewGuid());
}