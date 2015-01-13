namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RoarOfChallenge
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastLureOnRats()
      {
        Hand(P1, "Roar of Challenge");
        Battlefield(P1, "Typhoid Rats", "Grizzly Bears", "Grizzly Bears", "Forest", "Forest", "Forest");

        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(16, P2.Life);
        Equal(2, P1.Battlefield.Creatures.Count());
        Equal(2, P2.Battlefield.Creatures.Count());
      }

      [Fact]
      public void CastLureAndIndestructibleOnRats()
      {
        Hand(P1, "Roar of Challenge");
        Battlefield(P1, "Typhoid Rats", "Juggernaut", "Forest", "Forest", "Forest");

        P2.Life = 6;
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(1, P2.Life);
        Equal(2, P1.Battlefield.Creatures.Count());
        Equal(2, P2.Battlefield.Creatures.Count());
      }
    }
  }
}
