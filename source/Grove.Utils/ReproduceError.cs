namespace Grove.Utils
{
  using System;
  using Artifical;
  using Persistance;

  public class ReproduceError : Task
  {
    private readonly ErrorReportLoader _errorReportLoader;

    public ReproduceError(ErrorReportLoader errorReportLoader)
    {
      _errorReportLoader = errorReportLoader;
    }

    public override bool Execute(Arguments arguments)
    {
      var filename = arguments["f"];
      var rollback = int.Parse(arguments.TryGet("r") ?? "15");

      Console.WriteLine("Attach the debugger then press any key...");
      Console.ReadKey();

      var game = _errorReportLoader.LoadReport(filename, rollback, 
        new SearchParameters(10, 1, enableMultithreading: false));

      try
      {
        game.Start(numOfTurns: 30);
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return true;
      }

      Console.WriteLine("Everything was fine :)");
      return true;
    }

    public override void Usage()
    {
      Console.WriteLine(
       "usage: ugrove debug f=debug.report\n\nLoads error report named 'debug.report' and replays the game to the moment\nthe error has accured.");      
    }
  }
}