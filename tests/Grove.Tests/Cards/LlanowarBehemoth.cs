namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class LlanowarBehemoth
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BugKavuAttacksAndGetsBusted()
      {
        var kavu = C("Flametongue Kavu");

        Battlefield(P1, kavu);
        Battlefield(P2, "Llanowar Behemoth");

        RunGame(maxTurnCount: 1);

        Equal(Zone.Battlefield, C(kavu).Zone);
      }
    }

    public class Predefined : PredifinedScenario
    {
      [Fact]
      public void TapACreatureToGiveBehemoth11UntilEot()
      {
        var behemoth = C("Llanowar Behemoth");
        var bear = C("Grizzly Bears");

        Battlefield(P1, behemoth, bear);

        Exec(
          At(Step.Upkeep)
            .Activate(behemoth, costTarget: bear)
            .Activate(behemoth, costTarget: behemoth)
            .Verify(() => {
              True(C(behemoth).Power == 6 && C(behemoth).Toughness == 6);
              True(C(bear).IsTapped);
              True(C(behemoth).IsTapped);
            }),
          At(Step.Upkeep, turn: 2)
            .Verify(() => {
              Equal(4, C(behemoth).Power);
              Equal(4, C(behemoth).Toughness);
            }));
      }
    }
  }
}