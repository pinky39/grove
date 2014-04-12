namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PowderKeg
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyElves()
      {
        Battlefield(P1, "Powder Keg", "Grizzly Bears");
        Battlefield(P2, "Llanowar Elves", "Llanowar Elves", "Llanowar Elves");
        P2.Life = 2;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}