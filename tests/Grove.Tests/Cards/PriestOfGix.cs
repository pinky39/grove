namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class PriestOfGix
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Play3Priests()
      {
        Hand(P1, "Priest of Gix", "Priest of Gix", "Priest of Gix");
        Battlefield(P1, "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(3, P1.Battlefield.Creatures.Count());
      }
    }
  }
}