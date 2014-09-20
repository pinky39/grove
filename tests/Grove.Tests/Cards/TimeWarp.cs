namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TimeWarp
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TakeExtraTurn()
      {
        var warp = C("Time Warp");
        Hand(P1, warp, "Swamp", "Swamp", "Grave Titan");
        Battlefield(P1, "Island", "Swamp", "Swamp", "Island", "Island", "Rumbling Slum");

        RunGame(2);

        Equal(8, P2.Life);
        Equal(Zone.Graveyard, C(warp).Zone);
      }
    }
  }
}