namespace Grove.Tests.Cards
{
  using System.Linq;
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class RavenousBaloth
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BugPlayBaloth()
      {
        var baloth = C("Ravenous Baloth");

        Hand(P1, "Acidic Slime", baloth, "Vines of Vastwood", "Vines of Vastwood");
        Hand(P2, "Forest", "Lightning Bolt");

        Battlefield(P2, "Forest", "Birds of Paradise", "Rootbound Crag", "Forest", "Rumbling Slum", "Forest");
        Battlefield(P1, "Forest", "Rootbound Crag", "Rootbound Crag", "Forest");

        RunGame(maxTurnCount: 1);

        Equal(Zone.Battlefield, C(baloth).Zone);
      }

      public class Predefined : PredefinedScenario
      {
        [Fact]
        public void SacBalothToGain4Life()
        {
          var baloth = C("Ravenous Baloth");

          Battlefield(P1, baloth);

          Exec(
            At(Step.FirstMain)
              .Activate(baloth, costTarget: baloth)
              .Verify(() =>
                {
                  Equal(24, P1.Life);
                  Equal(Zone.Graveyard, C(baloth).Zone);
                })
            );
        }
      }

      public class PredefinedAi : PredefinedAiScenario
      {
        [Fact]
        public void SacBalothInResponseToShock()
        {
          var shock = C("Shock");

          Battlefield(P2, "Ravenous Baloth");
          Battlefield(P1, "Mountain");
          Hand(P1, shock);

          P2.Life = 1;

          Exec(
            At(Step.EndOfTurn, turn: 2)
              .Cast(shock, P2)
              .Verify(() =>
                {
                  Equal(3, P2.Life);
                  Equal(1, P1.Graveyard.Count());
                })
            );
        }
      }
    }
  }
}