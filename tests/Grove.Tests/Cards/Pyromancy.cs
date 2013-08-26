namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Pyromancy
  {
    public class Ai : AiScenario 
    {
      [Fact]
      public void Deal6Damage()
      {
        Hand(P1, "Shivan Dragon", "Shivan Dragon");
        Battlefield(P1, "Pyromancy", "Mountain", "Mountain", "Mountain");
        P2.Life = 6;
        
        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}