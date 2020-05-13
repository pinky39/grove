namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GarrukApexPredator
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CreatureAttackingGet55()
      {
        var garruk = C("Garruk, Apex Predator");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Battlefield(P1, garruk.AddCounters(9, CounterType.Loyality), bear1, bear2);
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 10;
        
        RunGame(1);
        
        Equal(1, C(garruk).Loyality);
        Equal(7, C(bear1).Power);
        Equal(7, C(bear2).Toughness);
        Equal(0, P2.Life);
      }
    }
  }
}