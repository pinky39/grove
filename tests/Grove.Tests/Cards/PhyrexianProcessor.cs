namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class PhyrexianProcessor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Create55Token()
      {
        Hand(P1, "Phyrexian Processor");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

        RunGame(3);

        Equal(15, P1.Life);
        Equal(1, P1.Battlefield.Creatures.Count(x => x.Power == 5 && x.Toughness == 5));
      }
    }
  }
}