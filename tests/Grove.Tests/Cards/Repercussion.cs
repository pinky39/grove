namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Repercussion
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal6Damage()
      {        
        Hand(P1, "Lightning Bolt");
        Battlefield(P1, "Repercussion", "Trained Armodon", "Mountain");
        Battlefield(P2, "Trained Armodon");
        P2.Life = 6;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}