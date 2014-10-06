namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SpectraWard
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveProtectionAnd22()
      {
        Hand(P1, "Spectra Ward");
        Battlefield(P1, "Grizzly Bears", "Swamp", "Plains", "Swamp", "Plains", "Swamp", "Plains");

        P2.Life = 4;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}
