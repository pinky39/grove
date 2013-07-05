namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;

  public static class EnumerableEx
  {
    public static IEnumerable<T1> GetDuplicates<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> selector)
    {
      return collection.GroupBy(selector).Where(group => group.Count() > 1).SelectMany(group => group);
    }

    public static bool None<T>(this IEnumerable<T> enumerable)
    {
      return enumerable == null || !enumerable.Any();
    }

    public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
      return !enumerable.Any(predicate);
    }

    public static T MaxElement<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
    {
      var maxRank = int.MinValue;
      var maxElement = default(T);

      foreach (var element in enumerable)
      {
        var rank = selector(element);

        if (rank > maxRank)
        {
          maxRank = rank;
          maxElement = element;
        }
      }

      return maxElement;
    }

    public static IEnumerable<T> ToEnumerable<T>(this T obj)
    {
      yield return obj;
    }

    public static T Pop<T>(this IList<T> list)
    {
      var popped = list.First();
      list.RemoveAt(0);
      return popped;
    }

    public static IList<T> ShuffleInPlace<T>(this IList<T> list, IList<int> permutation)
    {
      Debug.Assert(permutation.Count == list.Count,
        "Permutation and list must be of equal lenghts.");

      var listCopy = list.ToArray();
      for (var i = 0; i < permutation.Count; i++)
      {
        list[permutation[i]] = listCopy[i];
      }
      return list;
    }

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, T tail)
    {
      return head.Concat(Enumerable.Repeat(tail, 1));
    }
  }
}