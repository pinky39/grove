namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MonasterySwiftspear
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastSpellGet11()
      {
        Battlefield(P1, "Monastery Swiftspear", "Forest", "Forest", "Forest", "Forest");
        Hand(P1, "Titanic Growth", "Titanic Growth");

        P2.Life = 11;

        RunGame(1);

        Equal(0, P2.Life); // 1 (base power) + 4 (titanic growth) + 4 (titanic growth) + 1 counter + 1 counter
      }
    }
  }
}
