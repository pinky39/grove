namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AngelsTrumpet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal1DamageDoNotTapBear()
      {
        var bears = C("Grizzly Bears");
        var wall = C("Wall of Blossoms");
        
        Battlefield(P1, "Angel's Trumpet", bears, wall);

        RunGame(1);

        Equal(19, P1.Life);
        False(C(bears).IsTapped);
        True(C(wall).IsTapped);
      }
    }
  }
}