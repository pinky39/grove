namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Subversion
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void YouGain1OpponentLooses1()
      {
        Battlefield(P1, "Subversion");                
        RunGame(2);

        Equal(21, P1.Life);
        Equal(19, P2.Life);
      }
    }
  }
}