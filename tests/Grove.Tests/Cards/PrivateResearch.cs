namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PrivateResearch
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw1Card()
      {
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Private Research"));
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 2;

        RunGame(1);

        Equal(1, P1.Hand.Count);
      }
    }
  }
}