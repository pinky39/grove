namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class YavimayaElder
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacAtEot()
      {
        Battlefield(P1, "Yavimaya Elder", "Forest", "Forest");
        Library(P1, "Llanowar Elves", "Forest", "Swamp");

        RunGame(2);

        Equal(3, P1.Hand.Count);
      }
    }
  }
}