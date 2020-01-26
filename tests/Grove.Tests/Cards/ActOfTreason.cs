namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ActOfTreason
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainControlOfDragon()
      {
        var dragon = C("Shivan Dragon");

        Hand(P1, "Act of Treason");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, dragon.Tap());

        P2.Life = 7;

        RunGame(2);

        Equal(1, P2.Life);
        Equal(P2, C(dragon).Controller);
      }
    }
  }
}
