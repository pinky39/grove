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

        Equal(0, P1.Hand.Count);
        Equal(4, C(giant).Power);
      }

      [Fact]
      public void GiantComesIntoPlayWithTrigger()
      {
        var giant = C("Nightfire Giant");

        Hand(P1, giant);
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Mountain", "Mountain");

        RunGame(1);

        Equal(0, P1.Hand.Count);
        Equal(5, C(giant).Power);
      }

      [Fact]
      public void MountainComesIntoPlay()
      {
        var giant = C("Nightfire Giant");

        Library(P1, "Mountain");
        Hand(P1, "Mountain");
        Battlefield(P1, giant, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        Library(P2, "Mountain");

        // P1 plays two Mountains
        RunGame(3);

        Equal(0, P1.Hand.Count);
        Equal(1, P2.Battlefield.Count);
        Equal(5, C(giant).Power);
      }
    }
  }
}
