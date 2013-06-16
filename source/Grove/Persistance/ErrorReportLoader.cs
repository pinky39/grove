namespace Grove.Persistance
{
  using Artifical;
  using Grove.Gameplay;
  using Grove.Gameplay.Misc;

  public class ErrorReportLoader
  {
    private readonly Game.IFactory _gameFactory;

    public ErrorReportLoader(Game.IFactory gameFactory)
    {
      _gameFactory = gameFactory;
    }

    public Game LoadReport(string filename, int rollback = 0, SearchParameters searchParameters = null)
    {
      var savedGame = (SavedGame) SaveLoadHelper.ReadData(filename);

      var game = _gameFactory.Create(GameParameters.Load(
        player1Controller: ControllerType.Machine,
        player2Controller: ControllerType.Machine,
        savedGame: savedGame,
        rollback: rollback,
        searchParameters: searchParameters));

      return game;
    }
  }
}