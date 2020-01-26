namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AbzanSkycaptain
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Put2CountersOnBear()
      {
        var bear = C("Grizzly Bears");
        
        Battlefield(P1, "Abzan Skycaptain", bear);
        Battlefield(P2, "Mountain", "Grizzly Bears");
        
        Hand(P2, "Shock");

        P2.Life = 2;

        RunGame(1);

        Equal(4, C(bear).Toughness);                
      }
    }
  }
}
