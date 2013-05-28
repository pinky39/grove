namespace Grove.Tests.Scenarios
{
  using System;
  using System.Diagnostics;
  using Grove.Infrastructure;
  using Infrastructure;
  using Xunit;

  public class GameFacts : Scenario
  {
    [Fact]
    public void HashOfCopyShouldNotChange1()
    {
      var gameCopy = new CopyService().CopyRoot(Game);

      var originalHash = Game.CalculateHash();
      var copyHash = gameCopy.CalculateHash();


      Assert.Equal(originalHash, copyHash);
    }

    [Fact]
    public void HashOfCopyShouldNotChange2()
    {
      var mountain = C("Mountain");
      Hand(P1, mountain);

      var calc = new HashCalculator();

      var cardCopy = new CopyService().CopyRoot(C(mountain));

      var originalHash = calc.Calculate(C(mountain));
      var copyHash = calc.Calculate(cardCopy);


      Assert.Equal(originalHash, copyHash);
    }

    [Fact]
    public void HashPerformance()
    {
      var count = 10000;

      // create a copy to remove proxies
      var game = new CopyService().CopyRoot(Game);

      var stopWatch = new Stopwatch();
      stopWatch.Start();

      for (var i = 0; i < count; i++)
      {
        game.CalculateHash();
      }

      stopWatch.Stop();
      Console.WriteLine("Hashing of {0} game objects took: {1} ms.", count, stopWatch.Elapsed.TotalMilliseconds);
    }

    [Fact]
    public void CopyPerformance()
    {
      var stopWatch = new Stopwatch();

      // fill the cache
      new CopyService().CopyRoot(Game);

      stopWatch.Start();

      var count = 100;
      for (var i = 0; i < count; i++)
      {
        new CopyService().CopyRoot(Game);
      }

      stopWatch.Stop();

      Console.WriteLine("Copying of {0} game objects took: {1} ms.", count, stopWatch.Elapsed.TotalMilliseconds);
    }

    [Fact]
    public void CopyPerformance2()
    {
      var stopWatch = new Stopwatch();
      
      var context = new SerializationContext(new object[]
        {
          DecisionSystem,
          CardsDatabase,
          Game.Ai
        });      

      stopWatch.Start();

      var count = 100;
      for (var i = 0; i < count; i++)
      {
        CopyService2.Copy(Game, context);
      }

      stopWatch.Stop();

      Console.WriteLine("Copying of {0} game objects took: {1} ms.", count, stopWatch.Elapsed.TotalMilliseconds);
    }

    public GameFacts()
    {
      InitZones();
    }

    private void InitZones()
    {
      Hand(P1, C("Swamp"), C("Stupor"), C("Nantuko Shade"), C("Cruel Edict"));
      Hand(P2, C("Mountain"), C("Raging Ravine"), C("Thrun, the Last Troll"), C("Vines of Vastwood"));

      Battlefield(P1, C("Swamp"), C("Swamp"), C("Swamp"), C("Nantuko Shade"), C("Wurmcoil Engine"));
      Battlefield(P2, C("Raging Ravine"), C("Rootbound Crag"), C("Mountain"), C("Fires of Yavimaya"), C("Rumbling Slum"),
        C("Thrun, the Last Troll"));
    }
  }
}