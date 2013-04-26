namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ArgothianElder
  {
    // the way mana abilities are used by ai
    // this can only be used in a limiting way.
    // e.g ai will never float mana, this can just be used
    // to play more spells, but not more costly spells.

    public class Ai : AiScenario
    {
      [Fact]
      public void UntapLandsToPlayMoreCreatures()
      {
        Hand(P1, "Grizzly Bears", "Grizzly Bears");
        Battlefield(P1, "Forest", "Forest", "Argothian Elder");

        RunGame(1);

        Equal(3, P1.Battlefield.Creatures.Count());
      }
    }
  }
}