namespace Grove.Tests.Cards
{
  using System.Linq;
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class WayLay
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SummonBlockers()
      {
        var thrun = C("Thrun, the Last Troll");
        var waylay = C("WayLay");

        Battlefield(P1, thrun);
        Battlefield(P2, "Plains", "Plains", "Plains");
        Hand(P2, waylay);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(thrun),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(thrun).Zone);
                Equal(1, P2.Battlefield.Creatures.Count());
              }),
          At(Step.FirstMain, 2)
            .Verify(() => Equal(0, P2.Battlefield.Creatures.Count()))
          );
      }
    }
  }
}