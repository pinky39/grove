namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GrimMonolith
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void UntapMonolithToCastDragon()
      {
        var dragon = C("Shivan Dragon");
        
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", C("Grim Monolith").Tap());        
        Hand(P1, dragon);

        RunGame(3);

        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }
  }
}