namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SpireOwl
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutMountainOnTop()
      {
        Hand(P1, "Spire Owl", "Shivan Raptor");
        Battlefield(P1, "Island", "Island");
        Library(P1, "Shivan Raptor", "Mountain");

        RunGame(3);
        Equal(16, P2.Life);
      }
    }
  }
}