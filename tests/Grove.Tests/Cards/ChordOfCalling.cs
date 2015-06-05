namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ChordOfCalling
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SearchForDragon()
      {
        var dragon = C("Shivan Dragon");
        
        Hand(P1, "Chord of Calling");
        Library(P1, "Mountain", dragon);
        
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears", 
          "Forest", "Mountain", "Forest", "Forest", "Mountain");

        Battlefield(P2, "Shivan Dragon");

        RunGame(2);

        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }
  }
}