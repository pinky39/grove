namespace Grove.Infrastructure
{
  using System.Collections.Generic;
  using System.Linq;

  public static class StackExtension
  {
    public static T Pop<T>(this IList<T> list)
    {
      var popped = list.First();
      list.RemoveAt(0);
      return popped;
    }
  }
}