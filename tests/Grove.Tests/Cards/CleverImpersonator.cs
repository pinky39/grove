namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CleverImpersonator
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CopyDragon()
      {
        var impersonator = C("Clever Impersonator");
        
        Hand(P1, impersonator);
        Battlefield(P1, "Island", "Island", "Island", "Island");
        Battlefield(P2, "Shivan Dragon");

        RunGame(1);

        Equal("Shivan Dragon", C(impersonator).Name);
      }
    }

  }
}