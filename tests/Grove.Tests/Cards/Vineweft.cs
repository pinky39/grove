namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Vineweft
  {
    public class Ai: AiScenario
    {
      [Fact]
      public void ReturnToHand()
      {
        var vineweft = C("Vineweft");

        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest");        
        Graveyard(P1, vineweft);

        RunGame(2);

        Equal(Zone.Hand, C(vineweft).Zone);
      }
    }
  }
}