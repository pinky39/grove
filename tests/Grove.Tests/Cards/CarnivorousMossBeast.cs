namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CarnivorousMossBeast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AddCounter()
      {
        Battlefield(P1, "Carnivorous Moss-Beast", "Forest", "Forest", "Forest", "Forest",
          "Forest", "Forest", "Forest");
        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}