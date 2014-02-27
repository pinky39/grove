namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FogBank
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void BlockDragon()
      {
        var dragon = C("Shivan Dragon");
        var bank = C("Fog Bank");

        Battlefield(P1, dragon);
        Battlefield(P2, bank);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Battlefield, C(bank).Zone);
              })
          );
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void CombatOnly()
      {
        var shock = C("Shock");
        var bank = C("Fog Bank");

        Hand(P1, shock);
        Battlefield(P2, bank);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: bank),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(bank).Zone))
        );
      }
    }
  }
}