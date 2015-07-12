namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Quickling
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnWizardToHand()
      {
        var quickling = C("Quickling");
        var wizard = C("Fugitive Wizard");

        Hand(P1, quickling);
        Battlefield(P1, "Island", "Island", wizard);

        RunGame(2);

        Battlefield(P2, "Wall of Frost");

        Equal(Zone.Battlefield, C(quickling).Zone);
        Equal(Zone.Hand, C(wizard).Zone);
      }
    }
  }
}