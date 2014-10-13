namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class HuntersAmbush
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PreventAllDamagesFromNonGreen()
      {
        Battlefield(P1, "Grizzly Bears", "Oreskos Swiftclaw", "Oreskos Swiftclaw");

        P2.Life = 4;
        Battlefield(P2, "Forest", "Forest", "Forest");
        Hand(P2, "Hunter's Ambush");

        RunGame(1);

        Equal(2, P2.Life);
      }
    }
  }
}
