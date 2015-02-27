namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AmphinPathmage
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void MageBecomesUnblockable()
      {
        Battlefield(P1, "Amphin Pathmage", "Island", "Plains", "Island");

        P2.Life = 3;
        Battlefield(P2, "Grizzly Bears", "Island", "Plains", "Island");

        RunGame(1);
        Equal(0, P2.Life);
      }
    }
  }
}
