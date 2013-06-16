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

    public override void Execute(Arguments arguments)
    {
      var filename = arguments["filename"];
      var rollback = int.Parse(arguments.TryGet("rollback") ?? "0");
      
      var game = _errorReportLoader.LoadReport(filename, rollback, new SearchParameters(10, 1, enableMultithreading: false));

      try
      {
        game.Start(numOfTurns: 30);
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return;
      }

      Console.WriteLine("No errors found.");
    }
  }
}