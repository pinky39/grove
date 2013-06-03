namespace Grove.Tests
{
  using System.IO;
  using Artifical;
  using Gameplay;
  using Gameplay.Misc;
  using Infrastructure;
  using Persistance;
  using Xunit;

  public class SaveGameFacts : Scenario
  {
    [Fact]
    public void Save()
    {
      var game = SimulateGame();

      using (var fileStream = new FileStream("test.savegame", FileMode.Create))
      {
        game.SaveTo(fileStream);
      }

      GameParameters p;

      using (var file = new FileStream("test.savegame", FileMode.Open))
      {
        p = GameParameters.Load(
          player1Controller: ControllerType.Machine,
          player2Controller: ControllerType.Machine,
          savedGame: SavedGame.Deserialize(file));
      }


      var game1 = GameFactory.Create(p);

      Assert.Equal(game.CalculateHash(), game1.CalculateHash());
    }

    private Game SimulateGame()
    {
      var p = GameParameters.Simulation(GetDeck("deck1.dec"), GetDeck("deck2.dec"),
        new SearchParameters(maxSearchDepth: 15, maxTargetCount: 2, enableMultithreading: true));

      var game = GameFactory.Create(p);

      game.Start(numOfTurns: 15);
      return game;
    }
  }
}