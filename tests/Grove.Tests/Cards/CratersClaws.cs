namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CratersClaws
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Ferocious()
      {
        Hand(P1, "Crater's Claws");
        Battlefield(P1, "Mountain", "Mountain", "Leatherback Baloth");

        P2.Life = 7;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void NotFerocious()
      {
        Hand(P1, "Crater's Claws");
        Battlefield(P1, "Mountain", "Mountain");

        P2.Life = 2;

        RunGame(1);

        Equal(1, P2.Life);
      }
    }
  }
}