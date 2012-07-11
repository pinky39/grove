namespace Grove.Tests.Cards
{
  using Core;
  using Core.Details.Mana;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class BirdsOfParadise
  {
    #region Nested type: Ai

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

    #endregion

    #region Nested type: Predefined

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void AddOneAnyManaToPool()
      {
        var bird = C("Birds of Paradise");

        Battlefield(P1, bird);

        Exec(
          At(Step.FirstMain)
            .Activate(bird)
            .Verify(() =>
              {
                True(P1.HasMana(ManaUnit.White.ToAmount()));
                True(P1.HasMana(ManaUnit.Blue.ToAmount()));
                True(P1.HasMana(ManaUnit.Black.ToAmount()));
                True(P1.HasMana(ManaUnit.Red.ToAmount()));
                True(P1.HasMana(ManaUnit.Green.ToAmount()));
              })
          );
      }
    }

    #endregion
  }
}