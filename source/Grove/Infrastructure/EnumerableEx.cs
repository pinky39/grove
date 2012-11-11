namespace Grove.Infrastructure
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public static class EnumerableEx
  {
    public static bool ContainsElements<T>(this List<T> collection, List<T> elements)
    {
      if (elements.Count > collection.Count)
        return false;

      var collectionOrdered = collection.OrderBy(x => x).ToArray();
      var elementsOrdered = elements.OrderBy(x => x).ToArray();

      var collectionIndex = 0;
      var elementsIndex = 0;

      while (collectionIndex < collection.Count && elementsIndex < elements.Count)
      {
        if ((collectionOrdered[collectionIndex]).Equals(elementsOrdered[elementsIndex]))
        {
          collectionIndex++;
          elementsIndex++;
          continue;
        }
        collectionIndex++;
      }

      return elementsIndex == elements.Count;
    }

    public static int CountElements(this IEnumerable enumerable)
    {
      return enumerable.Cast<object>().Count();
    }

    public static object FirstElement(this IEnumerable enumerable)
    {
      return enumerable.Cast<object>().First();
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
      foreach (var item in enumerable)
      {
        action(item);
      }
    }

    public static void ForEach<T>(this IEnumerable<object> enumerable, Action<T> action)
    {
      var items = enumerable.Where(x => x is T).Cast<T>();

      foreach (var item in items)
      {
        action(item);
      }
    }

    public static IEnumerable<T1> GetDuplicates<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> selector)
    {
      return collection.GroupBy(selector).Where(group => group.Count() > 1).SelectMany(group => group);
    }

    public static object LastElement(this IEnumerable enumerable)
    {
      return enumerable.Cast<object>().Last();
    }

    public static bool None<T>(this IEnumerable<T> enumerable)
    {
      return enumerable == null || !enumerable.Any();
    }

    public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
      return !enumerable.Any(predicate);
    }

    public static T Single<T>(this IEnumerable<object> enumerable)
    {
      return enumerable.Where(x => x is T).Cast<T>().Single();
    }

    public static IEnumerable<T> ToEnumerable<T>(this T obj)
    {
      yield return obj;
    }
  }
}