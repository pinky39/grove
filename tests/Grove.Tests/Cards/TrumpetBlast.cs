namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TrumpetBlast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackForKill()
      {
        Battlefield(P1, "Mountain", "Forest", "Forest", "Grizzly Bears", "Grizzly Bears");
        Hand(P1, "Trumpet Blast");
        P2.Life = 8;
        
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}