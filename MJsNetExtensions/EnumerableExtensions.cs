namespace MJsNetExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Summary description for EnumerableExtensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Interleaved merge with LINQ - take from both only elements in the lenth of the shorter enumeration. 
        /// Warning: this will skip trailing elements from first or second, if the enumerations have different lengths!
        /// See: https://stackoverflow.com/questions/7224511/interleaved-merge-with-linq
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">first enumeration</param>
        /// <param name="second">second enumeration</param>
        /// <returns></returns>
        public static IEnumerable<T> InterleaveEnumerationsOfEqualLength<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var result =
                    first
                        .Zip(second, (f, s) => new[] { f, s })
                        .SelectMany(f => f)
                ;

            return result;
        }

        /// <summary>
        /// Interleaved merge with LINQ - take all elements. 
        /// NOTE: this interleave will simply coninue with the longer sequence, if the enumerations have different lengths.
        /// E.g. for: var first = new string[] { "1", "2", "3", "4", "5" };
        /// var second = new string[] { "a", "b", "c" };
        /// the result would be: { "1", "a", "2", "b", "3", "c", "4", "5" }.
        /// Or for: var first = new string[] { "1", "2", "3" };
        /// var second = new string[] { "a", "b", "c", "d", "e" };
        /// the result would be: { "1", "a", "2", "b", "3", "c", "d", "e", }.
        /// See: https://dotnetfiddle.net/CbSXp2
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source1">source1 enumeration</param>
        /// <param name="source2">source2 enumeration</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Interleave<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            ArgumentNullException.ThrowIfNull(source1);
            ArgumentNullException.ThrowIfNull(source2);

            using var enumerator1 = source1.GetEnumerator();
            using var enumerator2 = source2.GetEnumerator();
            bool continue1st;
            bool continue2nd;

            do
            {
                continue1st = enumerator1.MoveNext();
                if (continue1st)
                {
                    yield return enumerator1.Current;
                }

                continue2nd = enumerator2.MoveNext();
                if (continue2nd)
                {
                    yield return enumerator2.Current;
                }
            }
            while (continue1st || continue2nd);
        }

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using the default equality comparison, i.e. item.Equals(value).
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="list">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value)
        {
            if (list != null)
            {
                int index = 0;
                foreach (var item in list)
                {
                    if (item.Equals(value))
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="list">A sequence in which to locate a value.</param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <param name="comparer">An equality comparer to compare values.</param> 
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, -1.</returns>
        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
        {
            Throw.IfNull(comparer, nameof(comparer));

            if (list != null)
            {
                int index = 0;
                foreach (var item in list)
                {
                    if (comparer.Equals(item, value))
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// An exception free and safe way to get a <paramref name="position"/> from <paramref name="list"/> or a <typeparamref name="T"/> default,
        /// if the <paramref name="list"/> is null, or if <paramref name="position"/> is less than 0 or exceeds the <paramref name="list"/> size,
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="list"/>.</typeparam>
        /// <param name="list">The list where a <paramref name="position"/> is to be got from.</param>
        /// <param name="position">The position to get from the <paramref name="list"/>.</param>
        /// <returns>The element on the <paramref name="position"/> in the <paramref name="list"/> or a <typeparamref name="T"/> default.</returns>
        public static T TryGetPositionOrDefault<T>(this IList<T> list, int position)
        {
            if (list == null ||
                position < 0 ||
                list.Count <= position
               )
            {
                return default(T);
            }

            return list[position];
        }

        /// <summary>
        /// Fail safe extension method returning an instance of <see cref="List{String}"/>, no matter if the <paramref name="source"/> is null or not.
        /// In the result all null, empty or whitespace entries are removed.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
#pragma warning disable CA1002
        public static List<string> ToNonNullList(this IEnumerable<string> source)
#pragma warning restore CA1002
        {
            return source?.Where(x => !string.IsNullOrWhiteSpace(x))?.ToList() ??
                   new List<string>();
        }

        /// <summary>
        /// Fail safe extension method returning an instance of <see cref="List{T}"/>, no matter if the <paramref name="source"/> is null or not.
        /// In the result all null entries are removed.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
#pragma warning disable CA1002
        public static List<TSource> ToNonNullList<TSource>(this IEnumerable<TSource> source)
#pragma warning restore CA1002
        {
            return source?.Where(x => x != null)?.ToList() ??
                   new List<TSource>();
        }

        /// <summary>
        /// Fail safe join to string extension method. It calls internally <see cref="string.Join{TSource}(string?, IEnumerable{TSource})"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="values">The values.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string JoinToString<TSource>(this IEnumerable<TSource> values, string separator)
        {
            return values == null ? 
                string.Empty : 
                string.Join(separator, values);
        }

        /// <summary>
        /// Almost fail safe join to string extension method. It calls internally <see cref="string.Join{TSource}(string?, IEnumerable{TSource})"/>
        /// Almost because I cannot guarantee that the <paramref name="selector"/> doesn't fail because e.g. a null reference exception if iterating a null entry of <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="values">The values.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static string JoinToString<TSource>(this IEnumerable<TSource> values, string separator, Func<TSource, object> selector)
        {
            if (selector == null)
            {
                return JoinToString(values, separator);
            }

            return values == null ?
                string.Empty :
                string.Join(separator, values.Select(selector));
        }
    }
}
