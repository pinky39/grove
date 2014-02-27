namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ThranWarMachine
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ThranAttacksAndIsKilled()
      {
        var thran = C("Thran War Machine");
        Battlefield(P1, thran, "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Verdant Force");

        RunGame(1);

        Equal(Zone.Graveyard, C(thran).Zone);
      }
    }
  }
}