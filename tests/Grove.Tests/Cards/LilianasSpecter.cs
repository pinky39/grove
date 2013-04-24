namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Grove.Core;
  using Infrastructure;
  using Xunit;

  public class LilianasSpecter
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PlayerDiscardsCard()
      {
        var specter = C("Liliana's Specter");

        Hand(P1, specter);
        Hand(P2, "Forest");

        Exec(
          At(Step.FirstMain)
            .Cast(specter)
            .Verify(() => Equal(0, P2.Hand.Count())));
      }
    }
  }
}