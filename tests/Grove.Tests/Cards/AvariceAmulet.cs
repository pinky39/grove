namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class AvariceAmulet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void OpponentGainsControlOfAmulet()
      {
        var amulet = C("Avarice Amulet");
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 4;        
        Battlefield(P2, C("Grizzly Bears").IsEquipedWith(amulet));

        RunGame(1);

        Equal(P1, C(amulet).Controller);        
      }
    }
  }
}
