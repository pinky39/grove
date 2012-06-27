namespace Grove.Tests.Cards
{
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class BloodVassal
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayDragon()
      {
        var dragon = C("Shivan Dragon");
        
        Hand(P1, dragon);
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Blood Vassal");

        RunGame(2);
        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }
  }
}