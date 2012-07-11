namespace Grove.Infrastructure
{
  public static class Chaining
  {
    public static readonly object Continue;
    public static readonly object Stop = new object();

    public static bool ShouldContinue(object result)
    {
      return result == Continue;
    }
  }
}