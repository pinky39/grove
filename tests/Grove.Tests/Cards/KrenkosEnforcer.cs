namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class KrenkosEnforcer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BearCannotBlockIt()
      {
        Battlefield(P1, "Krenko's Enforcer");

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void CreatureWithSameColorCanBlockIt()
      {
        Battlefield(P1, "Krenko's Enforcer");

        P2.Life = 2;
        Battlefield(P2, "Wall of Fire");

        RunGame(1);

        Equal(2, P2.Life);
      }
    }
  }
}