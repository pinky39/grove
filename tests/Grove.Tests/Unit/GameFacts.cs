namespace Grove.Tests.Unit
{
  using Grove.Infrastructure;
  using Infrastructure;
  using Xunit;

  public class GameFacts : Scenario
  {
    [Fact]
    public void HashOfCopyShouldNotChange()
    {
      Hand(P1, C("Swamp"), C("Stupor"), C("Nantuko Shade"), C("Cruel Edict"));
      Hand(P2, C("Mountain"), C("Raging Ravine"), C("Thrun, the Last Troll"), C("Vines of Vastwood"));

      Battlefield(P1, C("Swamp"), C("Swamp"), C("Swamp"), C("Nantuko Shade"), C("Wurmcoil Engine"));
      Battlefield(P2, C("Raging Ravine"), C("Rootbound Crag"), C("Mountain"), C("Fires of Yavimaya"), C("Rumbling Slum"),
        C("Thrun, the Last Troll"));
                  
      var gameCopy = new CopyService().CopyRoot(Game);

      var originalHash = Game.CalculateHash();
      var copyHash = gameCopy.CalculateHash();


      Assert.Equal(originalHash, copyHash);
    }
  }
}