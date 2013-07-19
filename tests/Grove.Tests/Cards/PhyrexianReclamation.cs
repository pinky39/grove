namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PhyrexianReclamation
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnRaptorToHand()
      {
        Battlefield(P1, "Phyrexian Reclamation", "Swamp", "Mountain", "Mountain", "Mountain", "Mountain");
        Graveyard(P1, "Shivan Raptor");

        RunGame(1);

        Equal(17, P2.Life);
        Equal(18, P1.Life);
      }
    }
  }
}