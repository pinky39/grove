namespace Grove.Infrastructure
{
  using System;

  public static class AssertEx
  {
    public static void True(bool condition, string message)
    {
      if (!condition)
        throw new InvalidOperationException(message);
    }

    public static void Fail(string message)
    {
      True(false, message);
    }
  }
}