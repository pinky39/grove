namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TitaniasBoon
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutCountersOnBears()
      {
        Hand(P1, "Titania's Boon");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(3);

        Equal(2, P2.Life);
      }
    }
  }
}