namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GoblinHeelcutter
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Dash()
      {
        var goblin = C("Goblin Heelcutter");
        
        Hand(P1, goblin);
        Battlefield(P1, "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 4;

        RunGame(2);

        Equal(Zone.Hand, C(goblin).Zone);
        Equal(1, P2.Life);
      }

      [Fact]
      public void NoDash()
      {
        var goblin = C("Goblin Heelcutter");

        Hand(P1, goblin);
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 20;

        RunGame(2);

        Equal(Zone.Battlefield, C(goblin).Zone);
        Equal(20, P2.Life);
      }
    }
  }
}