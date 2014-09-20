namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Sicken
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillShade()
      {
        var shade = C("Looming Shade");
        
        Hand(P1, "Sicken");
        Battlefield(P1, "Swamp", "Swamp");        
        Battlefield(P2, shade, "Swamp");

        RunGame(2);

        Equal(Zone.Graveyard, C(shade).Zone);
      } 
    }
  }
}