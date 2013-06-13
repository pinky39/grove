namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Umbilicus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EachPlayerReturnALandToHand()
      {

        Hand(P1, "Island");
        Hand(P2, "Island");

        Battlefield(P1, "Umbilicus", "Grizzly Bears", "Island", "Island", "Island", "Island", "Island", "Island", "Island", "Island");
        Battlefield(P2, "Grizzly Bears", "Island", "Island", "Island", "Island", "Island", "Island", "Island", "Island");

        RunGame(2);

        Equal(1, P1.Hand.Count);
        Equal(2, P2.Hand.Count);
      }
    }
  }
}