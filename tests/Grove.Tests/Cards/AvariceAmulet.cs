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
      public void GetControlOnAmulet()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 4;
        Battlefield(P2, C("Grizzly Bears").IsEquipedWith("Avarice Amulet"));

        RunGame(1);

        Equal(2, P2.Life);
        Equal(0, P2.Battlefield.Count);
        Equal(2, P1.Battlefield.Count);
        Equal(1, P1.Battlefield.Creatures.Count());
      }
    }
  }
}
