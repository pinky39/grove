namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BloodVassal
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayEngine()
      {
        var engine = C("Wurmcoil Engine");

        Hand(P1, engine);
        Battlefield(P2, "Llanowar Behemoth");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Blood Vassal");

        RunGame(2);
        Equal(Zone.Battlefield, C(engine).Zone);
      }    
    }
  }
}