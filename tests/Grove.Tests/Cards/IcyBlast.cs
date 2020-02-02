namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class IcyBlast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Ferocious()
      {
        Hand(P1, "Icy Blast");
        Battlefield(P1, "Leatherback Baloth", "Island", "Island", "Island");
        Battlefield(P2, "Shivan Dragon", "Shivan Dragon");

        P2.Life = 8;
        RunGame(3);

        Equal(0, P2.Life);
      }
    }
  }
}