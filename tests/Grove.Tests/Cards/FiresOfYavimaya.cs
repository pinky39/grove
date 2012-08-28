namespace Grove.Tests.Cards
{
  using System.Linq;
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class FiresOfYavimaya
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void FiresOfYavimayaGivesBearHaste()
      {
        var bear = C("Grizzly Bears");
        Battlefield(P1, "Forest", "Forest", "Fires of Yavimaya");
        Hand(P1, bear);

        RunGame(maxTurnCount: 2);

        Equal(Zone.Battlefield, C(bear).Zone);
        True(C(bear).Has().Haste);
        Equal(18, P2.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void CreaturesHaveHaste()
      {
        var fires = C("Fires of Yavimaya");
        var bear = C("Grizzly Bears");

        Hand(P1, fires);
        Battlefield(P1, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(fires)
            .Verify(() => {
              Equal(Zone.Battlefield, C(fires).Zone);
              True(C(bear).Has().Haste);
            })
          );
      }

      [Fact]
      public void SacToGiveTargetPlus2Plus2()
      {
        var fires = C("Fires of Yavimaya");
        var bear = C("Grizzly Bears");

        Battlefield(P1, fires, bear);

        Exec(
          At(Step.FirstMain)
            .Activate(fires, target: bear)
            .Verify(() => {
              Equal(4, C(bear).Power);
              Equal(4, C(bear).Toughness);
              Equal(Zone.Graveyard, C(fires).Zone);
              False(C(bear).Has().Haste);
            })
          );
      }

      [Fact]
      public void WhenCreatureGoesToGraveyardItNoLongerHasHaste()
      {
        var fires = C("Fires of Yavimaya");
        var bear = C("Grizzly Bears");
        var shock = C("Shock");

        Hand(P1, fires);
        Battlefield(P1, bear);
        Hand(P2, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(fires)
            .Verify(() => {
              Equal(Zone.Battlefield, C(fires).Zone);
              True(C(bear).Has().Haste);
            }),
          At(Step.SecondMain)
            .Cast(shock, target: bear)
            .Verify(() => False(C(bear).Has().Haste))
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void BugNoHasteWith2Fires()
      {
        Hand(P1, "Lightning Bolt", "Burst Lightning", "Vines of Vastwood", "Leatherback Baloth", "Burst Lightning", "Vines of Vastwood", "Thrun, the Last Troll");
        Hand(P2, "Ravenous Baloth", "Vines of Vastwood", "Thrun, the Last Troll", "Lightning Bolt", "Rumbling Slum");

        Battlefield(P1, "Raging Ravine");
        Battlefield(P2, "Copperline Gorge", "Raging Ravine", "Forest", "Forest", "Fires of Yavimaya", "Fires of Yavimaya");

        Exec(
          At(Step.SecondMain, turn: 2)
            .Verify(() => Equal(12, P1.Life))
          );
      }
    }
  }
}