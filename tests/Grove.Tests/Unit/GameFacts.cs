using System;
using System.Diagnostics;
using Grove.Infrastructure;
using Grove.Tests.Infrastructure;
using Xunit;

namespace Grove.Tests.Unit
{
  public class GameFacts : Scenario
  {
    [Fact]
    public void HashOfCopyShouldNotChange()
    {
      InitZones();

      var gameCopy = new CopyService().CopyRoot(Game);

      var originalHash = Game.CalculateHash();
      var copyHash = gameCopy.CalculateHash();


      Assert.Equal(originalHash, copyHash);
    }

    private void InitZones()
    {
      Hand(P1, C("Swamp"), C("Stupor"), C("Nantuko Shade"), C("Cruel Edict"));
      Hand(P2, C("Mountain"), C("Raging Ravine"), C("Thrun, the Last Troll"), C("Vines of Vastwood"));

      Battlefield(P1, C("Swamp"), C("Swamp"), C("Swamp"), C("Nantuko Shade"), C("Wurmcoil Engine"));
      Battlefield(P2, C("Raging Ravine"), C("Rootbound Crag"), C("Mountain"), C("Fires of Yavimaya"), C("Rumbling Slum"),
                  C("Thrun, the Last Troll"));
    }

    [Fact]
    public void CopyPerformance()
    {
      InitZones();

      var stopWatch = new Stopwatch();

      // fill the cache
      new CopyService().CopyRoot(Game);  
      
      stopWatch.Start();

      var numOfInstances = 100;
      for (int i = 0; i < numOfInstances; i++)
      {
        new CopyService().CopyRoot(Game);  
      }

      stopWatch.Stop();

      Console.WriteLine("Copying took: {0} ms/instance.", stopWatch.Elapsed.TotalMilliseconds / numOfInstances);

    }
  }
}