namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class WhiskAway
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutDragonOnTopOfLibrary()
      {
        var dragon = C("Shivan Dragon");

        Hand(P1, "Whisk Away");
        Battlefield(P1, "Shivan Dragon", "Shivan Dragon", "Island", "Island", "Island");
        
        Battlefield(P2, dragon);

        P2.Life = 10;

        Equals(5, P2.Life);
        Equals(Zone.Library, C(dragon).Zone);
        Equals(2, P1.Battlefield.Count(x => x.Name == "Shivan Dragon"));
      }
    }
  }
}