namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SultaiSoothsayer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutForceInHandOtherIntoGraveyard()
      {
        var force = C("Verdant Force");

        Hand(P1, "Sultai Soothsayer");
        Library(P1, "Forest", "Forest", "Forest", force);
        Battlefield(P1, "Swamp", "Island", "Island", "Island", "Forest", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(Zone.Hand, C(force).Zone);
        Equal(new[] { "Forest", "Forest", "Forest" }, P1.Graveyard.Take(3).Select(x => x.Name).ToArray());
      }
    }
  }
}
