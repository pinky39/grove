namespace Grove.Infrastructure
{
  using System;

  public static class ParseExtensions
  {
    public static int? ParseInt(this string source)
    {
      int i;

      if (Int32.TryParse(source, out i))
        return i;

      return null;
    }
  }
}