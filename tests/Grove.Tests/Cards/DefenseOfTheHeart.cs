namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DefenseOfTheHeart
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Search2Creatures()
      {
        var force1 = C("Verdant Force");
        var force2 = C("Verdant Force");
        var heart = C("Defense of the Heart");
        
        Library(P1, "Forest", force1, force2);        
        Battlefield(P1, heart);
        Battlefield(P2, "Grizzly Bears", "Llanowar Elves", "Birds of Paradise");

        RunGame(1);

        Equal(Zone.Battlefield, C(force1).Zone);
        Equal(Zone.Battlefield, C(force2).Zone);
        Equal(Zone.Graveyard, C(heart).Zone);
      }
    }
  }
}