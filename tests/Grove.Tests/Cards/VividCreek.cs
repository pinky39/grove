namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class VividCreek
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DoNotUseGiveAnyMana()
      {
        var creek = C("Vivid Creek");
        var baloth = C("Ravenous Baloth");

        Hand(P1, creek, baloth);
        Battlefield(P1, "Forest", "Rootbound Crag", "Swamp");

        RunGame(3);

        Equal(2, C(creek).Counters);
        Equal(Zone.Battlefield, C(creek).Zone);
        Equal(Zone.Battlefield, C(baloth).Zone);
      }

      [Fact]
      public void UseAnyManaAbility()
      {
        var creek = C("Vivid Creek");
        var baloth = C("Ravenous Baloth");

        Hand(P1, creek, baloth);
        Battlefield(P1, "Swamp", "Rootbound Crag", "Swamp");

        RunGame(3);

        Equal(1, C(creek).Counters);
        Equal(Zone.Battlefield, C(creek).Zone);
        Equal(Zone.Battlefield, C(baloth).Zone);
      }
    }
  }
}