namespace Grove.Infrastructure
{
  using System.Diagnostics;
  using log4net;

  public static class Log
  {
    private static readonly ILog Logger = LogManager.GetLogger("Global");
    
    [Conditional("DEBUG")]
    public static void Debug(string message, params object[] args)
    {
      Logger.DebugFormat(message, args);
    }

  }
}