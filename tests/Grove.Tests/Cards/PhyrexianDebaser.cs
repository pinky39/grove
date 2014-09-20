namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PhyrexianDebaser
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillBlocker()
      {
        Battlefield(P1, "Phyrexian Debaser", "Shivan Dragon");
        Battlefield(P2, "Angelic Page");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}