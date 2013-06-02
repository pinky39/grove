namespace Grove.Tests
{
  using System.IO;
  using Gameplay;
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
        game.Save(fileStream);  
      }


      using (var fileStream = new FileStream("test.savegame", FileMode.Open))
      {
        var game1 = Game.Load(fileStream, CardsDatabase, DecisionSystem);
        game1.Resume(numOfTurns: 15);
      
        Assert.Equal(game.Score, game1.Score);
      }
    }

    private Game SimulateGame()
    {
      var game = Game.NewSimulation(GetDeck("deck1.dec"), GetDeck("deck2.dec"), CardsDatabase, DecisionSystem);      
      game.Start(numOfTurns: 15);
      return game;
    }
  }
}