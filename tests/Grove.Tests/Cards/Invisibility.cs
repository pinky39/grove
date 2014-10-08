namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Invisibility
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantedCannotBeBlockedWithBear()
      {
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Invisibility"));

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void EnchantedIsBlockedWithWall()
      {
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Invisibility", "Battle Mastery"));

        P2.Life = 2;
        Battlefield(P2, "Wall of Mulch");

        RunGame(1);

        Equal(2, P2.Life);
        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}
