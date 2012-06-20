namespace Grove.Tests.Cards
{
  using System.Linq;
  using Grove.Core;
  using Infrastructure;
  using Xunit;

  public class Stupor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayStuporIfOpponentHasAtLeast2CardsInHand()
      {
        Battlefield(P1, "Swamp", "Swamp", "Swamp");
        Hand(P1, "Stupor");
        Hand(P2, "Swamp", "Swamp");

        RunGame(maxTurnCount: 2);

        Equal(2, P2.Graveyard.Count());
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void OpponentDiscardsCardAtRandomThenDiscardsACard()
      {
        var stupor = C("Stupor");
        Hand(P1, stupor);
        Hand(P2, C("Forest"), C("Forest"));

        Exec(
          At(Step.FirstMain)
            .Cast(stupor)
            .Verify(() => Equal(0, P2.Hand.Count()))
          );
      }
    }
  }
}