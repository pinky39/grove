namespace Grove.Utils
{
  using System;
  using System.IO;
  using System.Reflection;
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
      runner.Run(args[0], args);
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