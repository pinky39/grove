namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class DerangedHermit
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Create4Tokens()
      {
        Hand(P1, "Deranged Hermit");
        
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest");
        RunGame(1);

        Equal(4, P1.Battlefield.Count(c => c.Is("squirrel") && c.Power == 2));
      }
    }
  }
}