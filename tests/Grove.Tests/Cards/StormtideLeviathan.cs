namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class StormtideLeviathan
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AllLandsAreIslands()
      {
        var forest = C("Forest");
        var wizard = C("Fugitive Wizard");

        Hand(P1, wizard);
        Battlefield(P1, "Stormtide Leviathan", forest);
        Battlefield(P2, "Wall of Denial", "Mountain");
        
        RunGame(1);

        Equal(12, P2.Life);        
        Equal(Zone.Battlefield, C(wizard).Zone);
        True(C(forest).Is("island"));
      }
    }
  }
}