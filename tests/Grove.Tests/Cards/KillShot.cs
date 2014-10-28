namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class KillShot
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestrpyAttackingDragon()
      {
        Battlefield(P1, "Shivan Dragon");

        Hand(P2, "Kill Shot");
        Battlefield(P2, "Plains", "Plains", "Plains");

        P2.Life = 5;

        RunGame(1);

        Equal(5, P2.Life);
        Equal(1, P1.Graveyard.Count);
      }
    }
  }
}
