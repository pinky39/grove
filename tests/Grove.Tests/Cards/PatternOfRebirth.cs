namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class PatternOfRebirth
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SearchForDragon()
      {
        var dragon = C("Shivan Dragon");
        
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Pattern of Rebirth"));        
        Library(P1, dragon);

        Hand(P2, "Shock");
        Battlefield(P2, "Mountain");
        P2.Life = 2;

        RunGame(1);

        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }
  }
}