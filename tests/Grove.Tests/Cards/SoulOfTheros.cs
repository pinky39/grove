namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SoulOfTheros
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AllCreaturesGet22()
      {
        Battlefield(P1, "Soul Of Theros", "Grizzly Bears", "Forest", "Forest", "Forest", "Plains", "Plains", "Plains");

        RunGame(1);

        Equal(8, P2.Life);
      }

      [Fact]
      public void ExileAndAllCreaturesGet22()
      {
        Battlefield(P1, "Grizzly Bears", "Forest", "Forest", "Forest", "Plains", "Plains", "Plains");
        Graveyard(P1, "Soul Of Theros");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
        Equal(0, P1.Graveyard.Count);
      }
    }
  }
}
