namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Outmaneuver
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackForKill()
      {
        Hand(P1, "Outmaneuver");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Trained Armodon", "Trained Armodon");
        Battlefield(P2, "Fog Bank", "Fog Bank");

        P2.Life = 6;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}