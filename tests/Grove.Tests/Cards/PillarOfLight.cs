namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PillarOfLight
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExileAngel()
      {
        var angel = C("Baneslayer Angel");

        Hand(P1, "Pillar of Light");
        Battlefield(P1, "Kinsbaile Skirmisher", "Plains", "Plains", "Plains");

        P2.Life = 2;

        Battlefield(P2, angel);

        RunGame(1);

        Equal(0, P2.Life);
        Equal(Zone.Exile, C(angel).Zone);
      }
    }
  }
}