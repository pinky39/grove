namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RadiantsJudgment
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillForce()
      {
        var force = C("Verdant Force");
        
        Hand(P1, "Radiant's Judgment");
        Battlefield(P1, "Plains", "Plains", "Plains");        
        Battlefield(P2, force);

        RunGame(1);

        Equal(Zone.Graveyard,C(force).Zone);
      }
    }
  }
}