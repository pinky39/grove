namespace Grove.Infrastructure
{
  using System;

  public static class Asrt
  {
    public static void True(bool condition, string message)
    {
      if (!condition)
        throw new InvalidOperationException(message);
    }

    public static void Fail(string message, params object[] args)
    {
      True(false, String.Format(message, args));
    }

    public static void False(bool condition, string message)
    {
      True(!condition, message);
    }
  }
}