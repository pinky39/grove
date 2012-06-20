namespace Grove.Tests.Cards
{
  using Grove.Core;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class Vigor
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DamagePrevention()
      {
        var vigor = C("Vigor");
        var bear = C("Grizzly Bears");
        var shock = C("Shock");

        Battlefield(P1, bear, vigor);
        Hand(P2, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, bear)
            .Verify(() => {
              Equal(4, C(bear).Toughness);
              Equal(4, C(bear).Power);
              Equal(0, C(bear).Damage);
            })
          );
      }

      [Fact]
      public void GoesToLibraryInsteadOfGraveyard()
      {
        var vigor = C("Vigor");
        var stupor = C("Stupor");

        Hand(P1, stupor);
        Hand(P2, vigor);

        Exec(
          At(Step.FirstMain)
            .Cast(stupor)
            .Verify(() => Equal(Zone.Library, C(vigor).Zone))
          );
      }

      [Fact]
      public void Trample()
      {
        var vigor = C("Vigor");
        var bear = C("Grizzly Bears");

        Battlefield(P1, vigor);
        Battlefield(P2, bear);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(vigor),
          At(Step.DeclareBlockers)
            .DeclareBlockers(vigor, bear),
          At(Step.SecondMain)
            .Verify(() => Equal(16, P2.Life))
          );
      }
    }
  }

  public class Ai : AiScenario
  {
    [Fact]
    public void PumpCreaturesWithVigor()
    {
      Battlefield(P1, "Vigor", "Grizzly Bears", "Grizzly Bears", "Mountain", "Mountain", "Forest");
      Hand(P1, "Volcanic Fallout");

      Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

      RunGame(maxTurnCount: 2);
      Equal(4, P2.Life);
    }
  }
}