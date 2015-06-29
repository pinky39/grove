namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ElvishPiper
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutDragonIntoPlay()
      {
        Hand(P1, "Shivan Dragon");
        Battlefield(P1, "Elvish Piper", "Forest", "Fires of Yavimaya");
        P2.Life = 5;

        RunGame(1);

        Equal(-2, P2.Life);
      }
    }
  }
}