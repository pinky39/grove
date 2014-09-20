namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DestructiveUrge
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacLand()
      {
        var mountain = C("Mountain");

        Hand(P1, "Destructive Urge");
        Battlefield(P2, mountain, "Raging Ravine");
        Battlefield(P1, "Grizzly Bears", "Mountain", "Mountain", "Mountain");

        RunGame(1);

        Equal(Zone.Graveyard, C(mountain).Zone);
      }
    }
  }
}