namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Falter
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackToKill()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Forest", "Mountain");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        Hand(P1, "Falter");

        P2.Life = 6;

        RunGame(1);
        Equal(0, P2.Life);
      }
    }
  }
}