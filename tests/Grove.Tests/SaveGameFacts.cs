namespace Grove.Tests
{
  using System.IO;
  using Artifical;
  using Gameplay;
  using Gameplay.Misc;
  using Infrastructure;
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

      var p = GameParameters.Load(
          player1Controller: ControllerType.Machine,
          player2Controller: ControllerType.Machine,
          filename: "test.savegame");

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