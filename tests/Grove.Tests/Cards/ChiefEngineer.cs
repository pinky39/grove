namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ChiefEngineer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastJuggernautForConvoke()
      {
        Hand(P1, "Juggernaut", "Darksteel Citadel");
        Battlefield(P1, "Chief Engineer", "Island", "Island");

        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P1.Hand.Count);
      }
    }
  }
}
