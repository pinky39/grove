namespace Grove.Infrastructure
{
  using System.Diagnostics;
  using log4net;

  public static class LogFile
  {
    private static readonly ILog Logger = LogManager.GetLogger("Global");

    
    public static void Error(string message, params object[] args)
    {
      if (args.Length == 0)
      {
        Logger.Error(message);
        return;
      }

      Logger.ErrorFormat(message, args);
    }
        
    public static void Info(string message, params object[] args)
    {
      if (args.Length == 0)
      {
        Logger.Info(message);
        return;
      }

      Logger.InfoFormat(message, args);
    }
    
    [Conditional("DEBUG")]
    public static void Debug(string message, params object[] args)
    {
      Logger.DebugFormat(message, args);
    }

  }
}