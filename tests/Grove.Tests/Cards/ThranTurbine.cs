namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ThranTurbine
  {
    public class Ai : AiScenario
    {      
      [Fact]
      public void PayEcho()
      {
        var raptor = C("Shivan Raptor");

        Battlefield(P1, "Thran Turbine", raptor, "Mountain");
        RunGame(1);

        Equal(Zone.Battlefield, C(raptor).Zone);
      }
    }
  }
}