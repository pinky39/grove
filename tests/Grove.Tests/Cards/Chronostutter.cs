namespace Grove.Tests.Cards
{
  using System.Linq;
  using AI.TimingRules;
  using Infrastructure;
  using Xunit;

  public class Chronostutter
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnBearOnTopLibrarySecond()
      {
        var bear = C("Grizzly Bears");
        Library(P1, "Juggernaut", "Juggernaut");
        Battlefield(P1, bear);        

        P2.Life = 2;
        Battlefield(P2, "Island", "Island", "Island", "Island", "Island", "Island");
        Hand(P2, "Chronostutter");

        RunGame(3);

        Equal(62, P1.Library.Count);
        True(C(bear).Zone == Zone.Library);
      }
    }
  }
}
