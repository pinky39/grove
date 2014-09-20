namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Momentum
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Get11ForEachCounter()
      {
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Momentum"));
        RunGame(1);

        Equal(17, P2.Life);
      }
    }
  }
}