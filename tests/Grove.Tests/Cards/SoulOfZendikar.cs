namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class SoulOfZendikar
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CreateToken()
      {
        Battlefield(P1, "Soul of Zendikar", "Forest", "Forest", "Forest", 
          "Forest", "Forest", "Forest");

        RunGame(2);

        Equal(1, P1.Battlefield.Count(c => c.Is().Token));
      }
    }
  }
}