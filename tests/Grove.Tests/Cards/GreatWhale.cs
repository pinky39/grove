namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class GreatWhale
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Untap7Lands()
      {
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island", "Island");
        Hand(P1, "Great Whale");

        RunGame(1);

        Equal(7, P1.Battlefield.Count(x => x.IsTapped == false && x.Is().Land));
      }
    }
  }
}