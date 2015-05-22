namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class KirdChieftain
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveSelfABoost()
      {
        Battlefield(P1, "Kird Chieftain", "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Llanowar Elves");
        P2.Life = 6;
        
        RunGame(1);

        Equal(1, P2.Life);
      }
    }
  }
}