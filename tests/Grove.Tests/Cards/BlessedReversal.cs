namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BlessedReversal
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Gain6Life()
      {
        Battlefield(P1, "Trained Armodon", "Trained Armodon");
        Hand(P2, "Blessed Reversal");
        Battlefield(P2, "Plains", "Plains");
        P2.Life = 6;
        
        RunGame(1);

        Equal(6, P2.Life);
      }
    }
  }
}