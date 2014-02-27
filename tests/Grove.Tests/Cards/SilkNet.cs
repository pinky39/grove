namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SilkNet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillDragon()
      {
        var dragon = C("Shivan Dragon");
        Battlefield(P1, dragon);
        Hand(P2, "Silk Net");
        Battlefield(P2, "Blanchwood Treefolk", "Forest");

        RunGame(1);

        Equal(Zone.Graveyard, C(dragon).Zone);
      }
    }
  }
}