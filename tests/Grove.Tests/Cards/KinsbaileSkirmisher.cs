namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class KinsbaileSkirmisher
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BearsGetP1P1()
      {
        Battlefield(P1, "Grizzly Bears", "Plains", "Plains");
        Hand(P1, "Kinsbaile Skirmisher");

        P2.Life = 3;

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count());
        Equal(0, P2.Life);
      }
    }
  }
}