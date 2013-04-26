namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class SkitteringSkirge
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void SkirgeDiesAfterPlayingBear()
      {
        var skirge = C("Skittering Skirge");
        var bear = C("Grizzly Bears");

        Battlefield(P1, skirge);
        Hand(P1, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(bear),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(skirge).Zone))
          );
      }
    }
  }
}