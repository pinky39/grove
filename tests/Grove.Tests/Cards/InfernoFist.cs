namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class InfernoFist
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacrificeInfernoDeal2Damage()
      {
        Battlefield(P1, "Mountain", "Swamp", C("Grizzly Bears").IsEnchantedWith("Inferno Fist"));

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
