namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ArcheryTraining
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal1DamageToBlocker()
      {
        var merfolk = C("Coral Merfolk");
        var roach = C("Giant Cockroach");
        var wall = C("Wall of Blossoms");

        Battlefield(P1, wall.IsEnchantedWith("Archery Training"), roach);        
        Battlefield(P2, merfolk);

        P2.Life = 4;

        RunGame(1);

        Equal(Zone.Graveyard, C(merfolk).Zone);
        Equal(Zone.Battlefield, C(roach).Zone);
        True(C(wall).IsTapped);
      }
    }
  }
}