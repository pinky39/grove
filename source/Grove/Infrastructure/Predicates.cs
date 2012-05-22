namespace Grove.Infrastructure
{
  using System;
  using System.Linq;

  public static class Predicates
  {
    public static Func<T, bool> And<T>(params Func<T, bool>[] predicates )
    {
      return (param) => predicates.All(predicate => predicate(param));
    }
  }
}