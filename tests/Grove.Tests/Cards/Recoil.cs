namespace Grove.Tests.Cards
{
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class Recoil
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BounceBlocker()
      {
        var engine = C("Wurmcoil Engine");
        
        Hand(P1, "Recoil");
        Battlefield(P1, "Swamp", "Swamp", "Island", "Ravenous Baloth");        
        Battlefield(P2, engine);

        RunGame(maxTurnCount: 1);
        Equal(16, P2.Life);
        Equal(Zone.Graveyard, C(engine).Zone);
      }
    }
  }
}