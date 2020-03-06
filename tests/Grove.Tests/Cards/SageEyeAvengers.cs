namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SageEyeAvengers
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnBearToHand()
      {
        Battlefield(P1, "Sage-Eye Avengers");
        Battlefield(P2, "Grizzly Bears", "Forest");

        P2.Life = 4;
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}