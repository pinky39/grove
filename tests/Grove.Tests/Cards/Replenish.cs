namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Replenish
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Return2Rancors()
      {
        Hand(P1, "Replenish");
        Battlefield(P1, "Fledgling Osprey", "Plains", "Island", "Island", "Island");
        Graveyard(P1, "Worship", "Rancor", "Rancor");
        Battlefield(P2, "Trained Armodon");
        P2.Life = 5;

        RunGame(1);

        Equal(0, P2.Life);
        Equal(3, P1.Battlefield.Count(c => c.Is().Enchantment));
      }

      [Fact]
      public void SickenCharger()
      {
        Hand(P1, "Replenish");
        Battlefield(P1, "Fledgling Osprey", "Plains", "Island", "Island", "Island");
        Graveyard(P1, "Rancor", "Sicken");
        Battlefield(P2, "Pegasus Charger");
        P2.Life = 3;

        RunGame(1);

        Equal(0, P2.Life);        
      }
    }
  }
}