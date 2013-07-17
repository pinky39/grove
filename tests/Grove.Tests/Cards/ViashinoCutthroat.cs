namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ViashinoCutthroat
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackAndReturnToHand()
      {
        var cutthroat = C("Viashino Cutthroat");
        
        Hand(P1, cutthroat);
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain");

        RunGame(1);

        Equal(15, P2.Life);
        Equal(Zone.Hand, C(cutthroat).Zone);
      }
    }
  }
}