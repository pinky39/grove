namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Incendiary
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DealDamageToPlayer()
      {
        var incendiary = C("Incendiary").AddCounters(2, CounterType.Fuse);
        
        Battlefield(P1, "Fodder Cannon", "Island", "Mountain", "Mountain", 
          "Mountain", C("Grizzly Bears").IsEnchantedWith(incendiary), "Trained Armodon");
        
        Battlefield(P2, "Trained Armodon");

        P2.Life = 6;

        RunGame(1);
        
        Equal(0, P2.Life);
      }
    }
  }
}