namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class BirdsOfParadise
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayBearWithBird()
      {
        var bear = C("Grizzly Bears");

        Battlefield(P1, "Mountain", "Birds of Paradise");
        Hand(P1, bear);

        RunGame(maxTurnCount: 2);
        Equal(Zone.Battlefield, C(bear).Zone);
      }
    }

    public class Predefined : PredifinedScenario
    {
      [Fact]
      public void AddOneAnyManaToPool()
      {
        var bird = C("Birds of Paradise");

        Battlefield(P1, bird);

        Exec(
          At(Step.FirstMain)
            .Activate(bird)
            .Verify(() => {
              True(P1.ManaPool.HasColor(ManaColors.White));
              True(P1.ManaPool.HasColor(ManaColors.Blue));
              True(P1.ManaPool.HasColor(ManaColors.Black));
              True(P1.ManaPool.HasColor(ManaColors.Red));
              True(P1.ManaPool.HasColor(ManaColors.Green));
            })
          );
      }
    }
  }
}