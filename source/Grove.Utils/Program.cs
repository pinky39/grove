namespace Grove.Utils
{
  using System;
  using System.IO;
  using System.Reflection;
  using Infrastructure;
  using Media;

  internal class Program
  {
    private static int Main(string[] args)
    {      
      var runner = new TaskRunner();

      try
      {
        if (args.Length < 1)
        {
          Usage();
          return 1;
        }
          

        if (!runner.Run(args))
        {          
          return 1;
        }
      }
      catch (Exception ex)
      {
        LogFile.Error(ex.ToString());
        return 1;
      }

      return 0;
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