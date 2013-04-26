namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class PriestOfTitania
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Add2Mana()
      {
        Hand(P1, "Elvish warrior");

        Battlefield(P1, "Priest of Titania");
        Battlefield(P2, "Llanowar Elves");

        RunGame(1);

        Equal(2, P1.Battlefield.Creatures.Count());
      }
    }
  }
}