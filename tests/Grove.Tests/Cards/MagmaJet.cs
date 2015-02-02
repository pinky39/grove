namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MagmaJet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutLandOnTopPutDragonOnBottom()
      {
        var mountain = C("Mountain");
        var dragon = C("Shivan Dragon");
        var elves = C("Llanowar Elves");

        Hand(P1, "Magma Jet");        
        Library(P1, dragon, mountain);
        Battlefield(P1, "Mountain", "Forest");        
        Battlefield(P2, elves);

        RunGame(1);

        Equal(Zone.Graveyard, C(elves).Zone);
        Equal(C(mountain), P1.Library.Top);
        Equal(C(dragon), P1.Library.Bottom);
      }
    }
  }
}