namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ManaLeech
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TapRavine()
      {
        var ravine = C("Raging Ravine");
        Battlefield(P1, "Mana Leech");
        Battlefield(P2, ravine, "Forest", "Forest", "Forest", "Mountain");

        RunGame(2);

        Equal(20, P1.Life);
        True(C(ravine).IsTapped);
      }
    }
  }
}