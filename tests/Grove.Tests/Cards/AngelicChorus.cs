namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AngelicChorus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainLife()
      {
        Hand(P1, "Grizzly Bears", "Elvish Warrior");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Angelic Chorus");
        RunGame(1);

        Equal(25, P1.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DoNotGainLifeWhenZoneIsNotBattlefield()
      {
        var bears = C("Grizzly Bears");

        Hand(P1, bears, "Angelic Chorus");

        Exec(
          At(Step.FirstMain)
            .Cast(bears)
            .Verify(() => Equal(20, P1.Life))
          );
      }
    }
  }
}