namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class Turnabout
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TapOpponentsCreaturesAndAttack()
      {
        Hand(P1, "Turnabout");

        Battlefield(P1, "Shivan Dragon", "Island", "Island", "Mountain", "Mountain");
        Battlefield(P2, "Angelic Wall", "Shivan Dragon");

        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void UntapCreaturesAndBlock()
      {
        var ascetic = C("Troll Ascetic");

        Hand(P2, "Turnabout");

        Battlefield(P1, ascetic);
        Battlefield(P2, C("Shivan Dragon").Tap(), "Island", "Island", "Island", "Island");

        P2.Life = 3;

        RunGame(1);

        Equal(3, P2.Life);
        Equal(Zone.Graveyard, C(ascetic).Zone);
      }
    }
  }
}