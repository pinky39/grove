namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SpectraWard
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveProtectionAnd22()
      {
        var bears = C("Grizzly Bears");

        Hand(P1, "Spectra Ward");        
        Battlefield(P1, bears, "Swamp", "Plains", "Swamp", "Plains", "Swamp", "Plains");

        P2.Life = 4;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        
        Equal(4, C(bears).Power);
        Equal(4, C(bears).Toughness);
        True(CardColors.All.All(x => C(bears).HasProtectionFrom(x)));
      }
    }
  }
}
