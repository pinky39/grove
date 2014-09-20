namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CapashenStandard
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacrificeStandardToDrawCard()
      {
        var bear1 = C("Grizzly Bears");
        Battlefield(P1, bear1.IsEnchantedWith("Capashen Standard"), "Plains", "Plains");
        
        Hand(P2, "Expunge");
        Battlefield(P2, "Swamp", "Swamp", "Swamp");
        P2.Life = 3;

        RunGame(1);

        Equal(3, P2.Life);
        Equal(1, P1.Hand.Count);
        Equal(Zone.Graveyard, C(bear1).Zone);
      }
    }
  }
}