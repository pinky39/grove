namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SoulOfNewPhyrexia
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackWithAll()
      {
        var soul = C("Soul of New Phyrexia");

        Battlefield(P1, soul,
          "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(2, P2.Life);
        Equal(0, P2.Battlefield.Creatures.Count());
        Equal(Zone.Battlefield, C(soul).Zone);
      }
    }
  }
}