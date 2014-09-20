namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FieldSurgeon
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SaveArmodon()
      {
        var armodon1 = C("Trained Armodon");
        var armodon2 = C("Trained Armodon");
        
        Battlefield(P1, armodon1, "Field Surgeon", "Wall of Blossoms");        
        Battlefield(P2, armodon2);
        P2.Life = 3;

        RunGame(1);

        Equal(Zone.Battlefield, C(armodon1).Zone);
        Equal(Zone.Graveyard, C(armodon2).Zone);        
      }
    }
  }
}