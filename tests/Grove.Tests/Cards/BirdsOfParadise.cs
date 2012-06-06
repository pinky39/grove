namespace Grove.Tests.Cards
{
  using Core;
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
            .Verify(() =>
              {
                True(P1.HasMana(Mana.White.ToAmount()));
                True(P1.HasMana(Mana.Blue.ToAmount()));
                True(P1.HasMana(Mana.Black.ToAmount()));
                True(P1.HasMana(Mana.Red.ToAmount()));
                True(P1.HasMana(Mana.Green.ToAmount()));
              })
          );
      }
    }

    #endregion
  }
}