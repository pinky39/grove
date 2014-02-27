namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TolarianWinds
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawANewHand()
      {
        var winds = C("Tolarian Winds");
        Hand(P1, winds, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island", "Island", "Island");

        RunGame(2);

        Equal(Zone.Graveyard, C(winds).Zone);
        Equal(6, P1.Hand.Count);
        Equal(7, P1.Graveyard.Count);
      }
    }
  }
}