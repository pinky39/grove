namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NetcasterSpider
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BlockCreatureWithFlyingIncreasePower()
      {
        Battlefield(P1, "Grizzly Bears", "Kapsho Kitefins");

        P2.Life = 3;
        Battlefield(P2, "Netcaster Spider");

        RunGame(1);

        Equal(1, P2.Life);
        Equal(1, P1.Battlefield.Count);
      }

      [Fact]
      public void BlockCreatureWithoutFlying()
      {
        Battlefield(P1, "Juggernaut");

        P2.Life = 5;
        Battlefield(P2, "Netcaster Spider");

        RunGame(1);

        Equal(5, P2.Life);
        Equal(1, P1.Battlefield.Count);
        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}
