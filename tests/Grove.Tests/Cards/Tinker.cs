namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Tinker
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacClawsOfGixForEngine()
      {
        var engine = C("Wurmcoil Engine");
        var gix = C("Claws of Gix");
        
        Hand(P1, "Tinker");        
        Battlefield(P1, gix, "Island", "Island", "Island");        
        Library(P1, "Swamp", "Swamp", engine);

        RunGame(1);

        Equal(Zone.Battlefield, C(engine).Zone);
        Equal(Zone.Graveyard, C(gix).Zone);
      }
    }
  }
}