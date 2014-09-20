namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class PeaceAndQuiet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyEmbraceWorship()
      {
        Hand(P1, "Peace And Quiet");
        Battlefield(P1, "Plains", "Plains");
        Battlefield(P2, C("Grizzly Bears").IsEnchantedWith("Gaea's Embrace"), "Worship");

        RunGame(2);

        Equal(2, P2.Graveyard.Count(c => c.Is().Enchantment));        
      }
    }
  }
}