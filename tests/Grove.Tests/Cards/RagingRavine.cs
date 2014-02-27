namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RagingRavine
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BugDestroyRavine()
      {
        var ravine = C("Raging Ravine");

        Battlefield(P1, "Forest", "Forest", "Mountain", "Forest", ravine);
        Battlefield(P2, "Swamp", "Swamp");
        Hand(P2, "Doom blade");

        RunGame(maxTurnCount: 2);

        Equal(Zone.Graveyard, C(ravine).Zone);
      }

      [Fact]
      public void ChangeToCreatureAndAttack()
      {
        Battlefield(P1, "Forest", "Forest", "Mountain", "Forest", "Raging Ravine");
        RunGame(maxTurnCount: 1);
        Equal(16, P2.Life);
      }

      [Fact]
      public void DoNotConsumeRavine()
      {
        Battlefield(P1, "Llanowar Elves", "Mountain", "Mountain", "Forest", "Raging Ravine");

        P2.Life = 10;

        RunGame(maxTurnCount: 1);
        Equal(6, P2.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void AttackFor4()
      {
        var ravine = C("Raging Ravine");

        Battlefield(P1, ravine);

        Exec(
          At(Step.FirstMain)
            .Activate(ravine, abilityIndex: 1),
          At(Step.DeclareAttackers)
            .DeclareAttackers(ravine),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(16, P2.Life);
                Equal(1, C(ravine).Counters);
              }),
          At(Step.FirstMain, turn: 2)
            .Verify(() =>
              {
                Null(C(ravine).Power);
                Null(C(ravine).Toughness);
              })
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DoNotChangeToCreatureIfCannotBlock()
      {
        var ravine = C("Raging Ravine");
        var charger1 = C("Pegasus Charger");
        var charger2 = C("Pegasus Charger");
        var charger3 = C("Pegasus Charger");

        Battlefield(P1, charger1, charger2, charger3);
        Battlefield(P2, ravine, "Forest", "Forest", "Forest", "Forest", "Forest");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(charger1, charger2, charger3),
          At(Step.SecondMain)
            .Verify(() => Equal(0, P2.Battlefield.Count(x => x.IsTapped)))
          );
      }
    }
  }
}