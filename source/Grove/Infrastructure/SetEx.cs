namespace Grove.Infrastructure
{
  using System.Collections.Generic;
  using System.Linq;

  public static class SetEx
  {
    public static List<T[]> KSubsets<T>(this IList<T> elements, int k)
    {
      var result = new List<T[]>();

      KSubsets(0, 0, k, elements, Enumerable.Empty<T>(),  result);
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
  }
}