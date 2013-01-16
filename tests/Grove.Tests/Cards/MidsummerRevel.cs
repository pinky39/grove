namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class MidsummerRevel
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Create3Tokens()
      {
        Battlefield(P1, "Midsummer Revel", "Forest");
        
        RunGame(6);

        Equal(3, P1.Battlefield.Creatures.Count());
      }
    }
  }
}