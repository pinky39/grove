namespace Grove.Tests.Cards
{
  using Gameplay.Mana;
  using Gameplay.States;
  using Gameplay.Zones;
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
                True(P1.HasMana(Mana.White));
                True(P1.HasMana(Mana.Blue));
                True(P1.HasMana(Mana.Black));
                True(P1.HasMana(Mana.Red));
                True(P1.HasMana(Mana.Green));
              })
          );
      }
    }
  }
}