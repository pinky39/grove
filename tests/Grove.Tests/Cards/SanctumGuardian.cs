namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class SanctumGuardian
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventDamageFromBolt()
      {
        ScenarioCard guardian = C("Sanctum Guardian");
        ScenarioCard bolt = C("Lightning Bolt");

        Hand(P1, bolt);
        Battlefield(P2, guardian);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, P2)
            .Activate(p =>
              {
                p.Card = guardian;
                p.Targets(E(bolt), P2);
              })
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Graveyard, C(guardian).Zone);
              })
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void TradeForBetterCreature()
      {
        ScenarioCard dragon = C("Shivan Dragon");
        ScenarioCard bolt1 = C("Lightning Bolt");
        ScenarioCard bolt2 = C("Lightning Bolt");

        Battlefield(P2, dragon, "Sanctum Guardian");
        Hand(P1, bolt1, bolt2);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt1, dragon)
            .Cast(bolt2, dragon),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Battlefield, C(dragon).Zone))
          );
      }

      [Fact]
      public void PreventCombatDamage()
      {
        ScenarioCard guardian = C("Sanctum Guardian");
        ScenarioCard baloth1 = C("Leatherback Baloth");
        ScenarioCard baloth2 = C("Leatherback Baloth");

        P2.Life = 4;

        Battlefield(P1, baloth1, baloth2);
        Battlefield(P2, guardian);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(baloth1, baloth2),
          At(Step.SecondMain)
            .Verify(() => Equal(4, P2.Life))
          );
      }
    }
  }
}