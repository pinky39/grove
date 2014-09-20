namespace Grove.Utils
{
  using System;
  using System.IO;
  using System.Reflection;
  using Infrastructure;
  using Media;

  internal class Program
  {
    private static void Main(string[] args)
    {
      MediaLibrary.LoadAll();
      var runner = new TaskRunner();

      try
      {
        if (!runner.Run(args))
        {
          Usage();
          return;
        }
      }
      catch (Exception ex)
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