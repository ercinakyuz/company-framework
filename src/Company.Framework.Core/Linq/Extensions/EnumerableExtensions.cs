namespace Company.Framework.Core.Linq.Extensions
{
    public static class EnumerableExtensions
    {
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
        {
            using var enumerator = enumerable.GetEnumerator();

            while (await Task.Run(enumerator.MoveNext).ConfigureAwait(false))
            {
                yield return enumerator.Current;
            }
        }

    }
}
