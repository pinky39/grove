namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NightfireGiant
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiantComesIntoPlayWithoutTrigger()
      {
        var giant = C("Nightfire Giant");

        Hand(P1, giant);
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(4, C(giant).Power);
      }

      [Fact]
      public void GiantComesIntoPlayWithTrigger()
      {
        var giant = C("Nightfire Giant");

        Hand(P1, giant);
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Mountain", "Mountain");

        RunGame(1);

        Equal(5, C(giant).Power);
      }

      [Fact]
      public void PlayMountainGiantGains11()
      {
        var giant = C("Nightfire Giant");

        Hand(P1, "Mountain");
        Battlefield(P1, giant, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        RunGame(1);
        Equal(5, C(giant).Power);
      }

      [Fact]
      public void DestroyMountainGiantLooses11()
      {
        var giant = C("Nightfire Giant");

        Hand(P1, "Avalanche Riders");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, giant, "Mountain");

        RunGame(1);
        Equal(4, C(giant).Power);
      }
    }
  }
}