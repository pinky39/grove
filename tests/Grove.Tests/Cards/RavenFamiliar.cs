namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class RavenFamiliar
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutForceInHandOtherOnBottom()
      {
        var force = C("Verdant Force");
        
        Hand(P1, "Raven Familiar");        
        Library(P1, "Forest", "Forest", force);
        Battlefield(P1, "Island", "Island", "Island", "Island", "Forest", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(Zone.Hand, C(force).Zone);
        Equal(new[] {"Forest", "Forest"}, P1.Library.Reverse().Take(2).Select(x => x.Name).ToArray());
      }
    }
  }
}