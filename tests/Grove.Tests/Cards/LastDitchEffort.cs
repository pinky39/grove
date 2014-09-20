namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class LastDitchEffort
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal3Damage()
      {
        Hand(P1, "Last-Ditch Effort");
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Mountain");
        P2.Life = 9;

        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}