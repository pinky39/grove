namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Sandblast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyDragon()
      {
        Battlefield(P1, "Shivan Dragon");

        P2.Life = 5;
        Hand(P2, "Sandblast");
        Battlefield(P2, "Forest", "Forest", "Plains");

        RunGame(1);

        Equal(0, P1.Battlefield.Count);
      }
    }
  }
}
