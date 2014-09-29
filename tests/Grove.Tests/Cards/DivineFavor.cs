namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DivineFavor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GetLifeAndCounter13()
      {
        Hand(P1, "Divine Favor");
        Battlefield(P1, "Grizzly Bears", "Swamp", "Plains");

        P2.Life = 3;

        RunGame(1);

        Equal(0, P2.Life);
        Equal(23, P1.Life);
      }
    }
  }
}