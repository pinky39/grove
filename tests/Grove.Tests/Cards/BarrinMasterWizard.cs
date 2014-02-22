namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class BarrinMasterWizard
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BounceDragonSacLand()
      {
        var dragon1 = C("Shivan Dragon");

        Battlefield(P1, dragon1, "Mountain", "Mountain");
        Battlefield(P2, "Barrin, Master Wizard", "Island", "Island");

        P2.Life = 5;
        RunGame(1);

        Equal(5, P2.Life);
        Equal(Zone.Hand, C(dragon1).Zone);
        Equal(1, P2.Graveyard.Count(x => x.Is().Land));
      }

      [Fact]
      public void BounceAttackerSacCreature()
      {
        Battlefield(P1, "Shivan Dragon", "Shivan Dragon", "Mountain", "Mountain");
        Battlefield(P2, "Barrin, Master Wizard", "Birds of Paradise", "Island", "Island");

        P2.Life = 5;
        RunGame(1);

        Equal(5, P2.Life);

        Equal(1, P1.Hand.Count(x => x.Is().Creature));
        Equal(2, P2.Graveyard.Count());
      }
    }
  }
}