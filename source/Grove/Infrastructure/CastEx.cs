namespace Grove.Infrastructure
{
  public static class NiceCast
  {
    public static T As<T>(this object obj)
    {
      return (T) obj;
    }
  }
}