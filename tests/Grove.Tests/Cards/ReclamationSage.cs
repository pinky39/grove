namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ReclamationSage
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyStaff()
      {
        var staff = C("Staff of the Death Magus");
        
        Hand(P1, "Reclamation Sage");
        Battlefield(P1, "Forest", "Forest", "Forest");        
        Battlefield(P2, staff);

        RunGame(1);

        Equal(Zone.Graveyard, C(staff).Zone);        
      }
    }
  }
}