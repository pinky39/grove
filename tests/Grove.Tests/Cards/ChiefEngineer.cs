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
        var juggernaut = C("Juggernaut");
        Hand(P1, juggernaut);
        Battlefield(P1, "Chief Engineer", "Island", "Island", "Grizzly Bears");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(Zone.Battlefield, C(juggernaut).Zone);
      }
    }
  }
}
