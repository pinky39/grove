namespace Grove.Infrastructure
{
  using System.Collections.Generic;

  public static class ListEx
  {
    public static IList<T> OrderByInPlace<T>(this IList<T> list)
    {      
      // todo
      
      //for (var i = list.Count - 1; i >= 1; i--)
      //{
      //  var j = NextFast(0, i);
      //  list.Swap(i, j);
      //}

      return list;
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
      var ith = list[i];

      list[i] = list[j];
      list[j] = ith;
    }
  }
}