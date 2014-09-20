namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CropRotation
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void FetchGorgeToKillOpponent()
      {
        Library(P1, "Forest", "Forest", "Mountain", "Shivan Gorge");
        Hand(P1, "Crop Rotation");
        Battlefield(P1, "Mountain", "Forest", "Forest", "Mountain");
        P2.Life = 1;
        
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}