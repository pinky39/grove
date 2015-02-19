namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PreeminentCaptain
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ArchersJoinTheAttack()
      {
        Hand(P1, "Elite Archers");
        Battlefield(P1, "Preeminent Captain");

        RunGame(1);

        Equal(15, P2.Life);
      }
    }
  }
}
