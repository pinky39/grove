namespace Grove.Gameplay
{
  using System.IO;
  using Artifical;
  using Misc;
  using Persistance;

  public class GameParameters
  {
    private GameParameters() {}
    public PlayerParameters Player1 { get; private set; }
    public PlayerParameters Player2 { get; private set; }    
    public SearchParameters SearchParameters { get; private set; }
    public SavedGame SavedGame { get; private set; }
    public ControllerType Player1Controller { get; private set; }
    public ControllerType Player2Controller { get; private set; }

    public bool IsSavedGame { get { return SavedGame != null; } }

    public static GameParameters StandardGame(PlayerParameters player1, PlayerParameters player2)
    {
      return new GameParameters
        {
          Player1 = player1,
          Player2 = player2,
          SearchParameters = new SearchParameters(40, 2, enableMultithreading: true),
          Player1Controller = ControllerType.Human,
          Player2Controller = ControllerType.Machine
        };
    }

    public static GameParameters Scenario(ControllerType player1Controller, ControllerType player2Controller)
    {
      return new GameParameters
        {
          Player1 = new PlayerParameters {Name = "Player1", Avatar = "Player1.png", Deck = Deck.CreateUncastable()},
          Player2 = new PlayerParameters {Name = "Player2", Avatar = "Player2.png", Deck = Deck.CreateUncastable()},
          Player1Controller = player1Controller,
          Player2Controller = player2Controller,
          SearchParameters = new SearchParameters(40, 2, enableMultithreading: true)
        };
    }

    public static GameParameters Simulation(Deck player1Deck, Deck player2Deck, SearchParameters searchParameters)
    {
      return new GameParameters
        {
          Player1 = new PlayerParameters {Name = "Player1", Avatar = "Player1.png", Deck = player1Deck},
          Player2 = new PlayerParameters {Name = "Player2", Avatar = "Player2.png", Deck = player2Deck},
          Player1Controller = ControllerType.Machine,
          Player2Controller = ControllerType.Machine,
          SearchParameters = searchParameters
        };
    }

    public static GameParameters Load(ControllerType player1Controller, ControllerType player2Controller, string filename)
    {
      using (var stream = new FileStream(filename, FileMode.Open))
      {
        var savedGame = GameRecorder.LoadGame(stream);
        savedGame.Decisions.Position = 0;
        
        return new GameParameters
        {
          Player1 = savedGame.Player1,
          Player2 = savedGame.Player2,
          Player1Controller = player1Controller,
          Player2Controller = player2Controller,          
          SearchParameters = new SearchParameters(40, 2, enableMultithreading: true),
          SavedGame = savedGame
        };  
      }                  
    }
  }
}