namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ResoluteArchangel
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void LifeBecomes20()
      {
        Hand(P1, "Resolute Archangel");
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");

        P1.Life = 10;

        RunGame(1);
        Equal(20, P1.Life);
      }

      [Fact]
      public void LifeDoesNotChange()
      {
        Hand(P1, "Resolute Archangel");
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");

        P1.Life = 22;

        RunGame(1);
        Equal(22, P1.Life);
      }
    }
  }
}