namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class EphemeralShields
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ShieldTheBear()
      {
        var bear = C("Grizzly Bears");
        Battlefield(P1, bear, "Wall of Frost", "Plains");
        Hand(P1, "Ephemeral Shields");

        P2.Life = 2;
        Battlefield(P2, "Swamp", "Swamp");
        Hand(P2, "Doom blade");

        RunGame(1);

        Equal(0, P2.Hand.Count);
        Equal(0, P2.Life);
        Equal(Zone.Battlefield, C(bear).Zone);
      }
    }
  }
}