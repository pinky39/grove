namespace Grove.Tests.Cards
{
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class SealOfFire
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SealTheBear()
      {
        var sealOfFire = C("Seal of Fire");
        var bear = C("Grizzly Bears");
        
        
        Hand(P1, sealOfFire);
        Battlefield(P1, "Mountain");        
        Battlefield(P2, bear);

        RunGame(1);
        Equal(Zone.Graveyard, C(sealOfFire).Zone);
        Equal(Zone.Graveyard, C(bear).Zone);
      }
    }

  }
}