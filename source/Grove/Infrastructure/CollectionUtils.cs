namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static class CollectionUtils
  {
    public static List<T[]> KSubsets<T>(this IList<T> elements, int k)
    {
      var result = new List<T[]>();

      KSubsets(0, 0, k, elements, Enumerable.Empty<T>(), result);
      return result;
    }

    private static void KSubsets<T>(int i, int j, int k, IList<T> elements, IEnumerable<T> current, List<T[]> result)
    {
      if (j == k)
      {
        result.Add(current.ToArray());
        return;
      }

      for (int l = i; l < elements.Count; l++)
      {
        KSubsets(l + 1, j + 1, k, elements, current.Concat(elements[l]), result);
      }
    }
    
    public static void Swap<T>(this IList<T> list, int i, int j)
    {
      var ith = list[i];

      list[i] = list[j];
      list[j] = ith;
    }

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

    public static IEnumerable<T> Rotate<T>(this IEnumerable<T> enumerable, int elementCount)
    {
      foreach (var element in enumerable.Skip(elementCount))
      {
        yield return element;
      }

      foreach (var element in enumerable.Take(elementCount))
      {
        yield return element;
      }
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

    public static T MinElement<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
    {
      var minRank = int.MaxValue;
      var minElement = default(T);

      foreach (var element in enumerable)
      {
        var rank = selector(element);

        if (rank < minRank)
        {
          minRank = rank;
          minElement = element;
        }
      }

      return minElement;
    }

    public static IEnumerable<T> Concat<T>(this T obj, IEnumerable<T> enumerable)
    {
      return obj.ToEnumerable().Concat(enumerable);
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
      Asrt.True(permutation.Count == list.Count,
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

    public static IEnumerable<IEnumerable<T>> Permutate<T>(this IEnumerable<T> source)
    {
      int length = source.Count();
      if (length != 0)
      {
        int index = 0;
        foreach (var item in source)
        {
          var allOtherItems = source.RemoveAt(index);
          foreach (var permutation in allOtherItems.Permutate())
          {
            yield return new[] {item}.Concat(permutation);
          }
          index++;
        }
      }
      else
      {
        yield return new T[0];
      }
    }

    public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> source, int indexToRemove)
    {
      int index = 0;
      foreach (T item in source)
      {
        if (index != indexToRemove)
        {
          yield return item;
        }
        index++;
      }
    }
  }
}