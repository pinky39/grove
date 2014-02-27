namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class BlacksSunZenith
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AddCounters()
      {
        var sun = C("Black Sun's Zenith");

        Hand(P1, sun);
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Forest", "Forest", "Elvish Warrior");
        Battlefield(P2, "Elvish Warrior", "Llanowar Elves", "Birds of Paradise");

        RunGame(maxTurnCount: 1);

        Equal(1, P1.Battlefield.Creatures.Count());
        Equal(1, P2.Battlefield.Creatures.Count());
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void AddCounters()
      {
        var warrior1 = C("Elvish Warrior");
        var warrior2 = C("Elvish Warrior");
        var warrior3 = C("Elvish Warrior");
        var sun = C("Black Sun's Zenith");

        Hand(P1, sun);
        Battlefield(P1, warrior1);
        Battlefield(P2, warrior2, warrior3);

        Exec(
          At(Step.FirstMain)
            .Cast(sun, x: 2)
            .Verify(() =>
              {
                Equal(1, C(warrior1).Toughness);
                Equal(1, C(warrior2).Toughness);
                Equal(1, C(warrior3).Toughness);
                Equal(0, C(warrior1).Power);
              }));
      }
    }
  }
}