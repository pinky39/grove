namespace Grove.Infrastructure
{
  using System.Collections.Generic;

  public static class ListEx
  {    
    public static void Swap<T>(this IList<T> list, int i, int j)
    {
      var ith = list[i];

      list[i] = list[j];
      list[j] = ith;
    }
  }
}