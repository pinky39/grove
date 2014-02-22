namespace Grove.Gameplay
{
  using System.IO;
  using AI;

  public static class ErrorReportLoader
  {
    public static Game LoadReport(string filename, int rollback = 0, SearchParameters searchParameters = null)
    {
      using (var stream = new FileStream(filename, FileMode.Open))
      {
        var saveGameFile = SavedGames.ReadFromStream(stream);

        var gameParameters = GameParameters.Load(
          player1Controller: ControllerType.Machine,
          player2Controller: ControllerType.Machine,
          savedGame: (SavedGame) saveGameFile.Data,
          rollback: rollback,
          searchParameters: searchParameters);

        return new Game(gameParameters);
      }
    }
  }
}