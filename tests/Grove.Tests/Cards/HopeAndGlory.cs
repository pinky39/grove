namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HopeAndGlory
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void UntapAndBlock()
      {
        Battlefield(P1, "Grizzly Bears", "Trained Armodon");

        Hand(P2, "Hope And Glory");
        Battlefield(P2, C("Trained Armodon").Tap(), C("Grizzly Bears").Tap(), "Plains", "Forest");
        P2.Life = 5;
         
        RunGame(1);

        Equal(2, P1.Graveyard.Count);
        Equal(1, P2.Graveyard.Count);

      }
    }
  }
}