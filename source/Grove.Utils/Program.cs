namespace Grove.Utils
{
  using System;
  using System.IO;
  using System.Reflection;
  using Infrastructure;
  using UserInterface;

  internal class Program
  {
    private static void Main(string[] args)
    {            
      if (args.Length < 1)
      {
        Usage();
        return;
      }
      
      MediaLibrary.LoadResources();
      var runner = new TaskRunner();
      
      LogFile.Info("Starting task...");

      try
      {
        runner.Run(args[0], args);
      }
      catch(Exception ex)
      {
        LogFile.Error(ex.ToString());
      }
    }

    private static void Usage()
    {
      var usageText =
       Assembly.GetExecutingAssembly().GetManifestResourceStream("Grove.Utils.Usage.txt");

      using (var reader = new StreamReader(usageText))
      {
        Console.WriteLine(reader.ReadToEnd());
      }
    }
  }
}