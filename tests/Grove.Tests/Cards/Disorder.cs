namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class Disorder
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DoNotCastDisorderWhenNoEffect()
      {
        var disorder = C("Disorder");

        Hand(P1, disorder);
        Battlefield(P1, "Mountain", "Mountain");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        RunGame(2);

        Equal(Zone.Hand, C(disorder).Zone);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DealDamageToWhiteCreaturesAndTheirControllers()
      {
        var disorder = C("Disorder");

        Hand(P1, disorder);
        Battlefield(P1, "Grizzly Bears");
        Battlefield(P2, "White Knight");

        Exec(
          At(Step.FirstMain)
            .Cast(disorder)
            .Verify(() =>
              {
                Equal(0, P1.Graveyard.Creatures.Count());
                Equal(20, P1.Life);
                Equal(1, P2.Graveyard.Count());
                Equal(18, P2.Life);
              })
          );
      }
    }
  }
}