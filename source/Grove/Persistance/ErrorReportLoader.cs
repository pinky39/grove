namespace Grove.Persistance
{
  using System.IO;
  using Artifical;
  using Gameplay;
  using Gameplay.Misc;

  public class ErrorReportLoader
  {
    private readonly Game.IFactory _gameFactory;

    public ErrorReportLoader(Game.IFactory gameFactory)
    {
      _gameFactory = gameFactory;
    }

    public Game LoadReport(string filename, int rollback = 0, SearchParameters searchParameters = null)
    {
      using (var stream = new FileStream(filename, FileMode.Open))
      {
        var saveGameFile = SavedGames.ReadFromStream(stream);

        var game = _gameFactory.Create(GameParameters.Load(
          player1Controller: ControllerType.Machine,
          player2Controller: ControllerType.Machine,
          savedGame: (SavedGame) saveGameFile.Data,
          rollback: rollback,
          searchParameters: searchParameters));

        return game;
      }
    }
  }
}