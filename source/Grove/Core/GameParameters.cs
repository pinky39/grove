namespace Grove
{
  using AI;

  public class GameParameters
  {
    private GameParameters() {}

    public PlayerParameters Player1 { get; private set; }
    public PlayerParameters Player2 { get; private set; }
    public SearchParameters SearchParameters { get; private set; }
    public SavedGame SavedGame { get; private set; }
    public PlayerType Player1Controller { get; private set; }
    public PlayerType Player2Controller { get; private set; }
    public int RollBack { get; private set; }
    public int? Looser { get; private set; }

    public bool IsSavedGame { get { return SavedGame != null; } }

    public static GameParameters Default(PlayerParameters player1, PlayerParameters player2)
    {
      return new GameParameters
        {
          Player1 = player1,
          Player2 = player2,
          SearchParameters = SearchParameters.Default,
          Player1Controller = PlayerType.Human,
          Player2Controller = PlayerType.Machine
        };
    }

    public static GameParameters Scenario(PlayerType player1Controller, PlayerType player2Controller,
      SearchParameters searchParameters)
    {
      return new GameParameters
        {
          Player1 = new PlayerParameters {Name = "Player1", Deck = Deck.CreateUncastable()},
          Player2 = new PlayerParameters {Name = "Player2", Deck = Deck.CreateUncastable()},
          Player1Controller = player1Controller,
          Player2Controller = player2Controller,
          SearchParameters = searchParameters
        };
    }

    public static GameParameters Simulation(Deck player1Deck, Deck player2Deck, SearchParameters searchParameters)
    {
      return new GameParameters
        {
          Player1 = new PlayerParameters {Name = "Player1", Deck = player1Deck},
          Player2 = new PlayerParameters {Name = "Player2", Deck = player2Deck},
          Player1Controller = PlayerType.Machine,
          Player2Controller = PlayerType.Machine,
          SearchParameters = searchParameters
        };
    }

    public static GameParameters Load(PlayerType player1Controller, PlayerType player2Controller,
      SavedGame savedGame, int? looser = null, int rollback = 0, SearchParameters searchParameters = null)
    {
      return new GameParameters
        {
          Player1 = savedGame.Player1,
          Player2 = savedGame.Player2,
          Player1Controller = player1Controller,
          Player2Controller = player2Controller,
          SearchParameters = searchParameters ?? SearchParameters.Default,
          SavedGame = savedGame,
          RollBack = rollback,
          Looser = looser
        };
    }
  }
}