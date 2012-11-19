namespace Grove.Utils
{
  using System;

  internal class Program
  {
    private static void Main(string[] args)
    {
      if (args.Length < 1)
      {
        Usage();
        return;
      }

      var runner = new TaskRunner();
      runner.Run(args[0], args);
    }

    private static void Usage()
    {
      Console.WriteLine("Please specify task name.");
    }
  }
}