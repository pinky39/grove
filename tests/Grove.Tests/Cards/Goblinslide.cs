namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Goblinslide
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutGoblinIntoPlay()
      {
        Hand(P1, "Volcanic Hammer");
        Battlefield(P1, "Goblinslide", "Mountain", "Mountain", "Mountain");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void DontPutGoblinIntoPlay()
      {
        Hand(P1, "Volcanic Hammer");
        Battlefield(P1, "Goblinslide", "Mountain", "Mountain");

        P2.Life = 4;

        RunGame(1);

        Equal(1, P2.Life);
      }
    }
  }
}