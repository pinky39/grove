namespace Grove.Infrastructure
{
  using System.Diagnostics;

  public static class AssertEx
  {    
    public static void True(bool condition, string message)
    {
#if DEBUG
      Debug.Assert(condition, message);
#else 
      if(!condition)
        throw new InvalidOperationException(message);
#endif
    }    

    public static void Fail(string message)
    {
      True(false, message);
    }
  }
}