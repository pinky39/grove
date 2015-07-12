namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ConeOfFlame
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyAllOpponentsCreatures()
      {        
        Hand(P1, "Cone Of Flame");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Grizzly Bears");
        Battlefield(P2, "Juggernaut", "Skittering Skirge", "Llanowar Elves");

        RunGame(1);

        Equal(3, P2.Graveyard.Creatures.Count());
        Equal(0, P1.Graveyard.Creatures.Count());
      }

      [Fact]
      public void Destroy2CreaturesDeal1DamageToOpponent()
      {
        Hand(P1, "Cone Of Flame");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Juggernaut", "Skittering Skirge");

        RunGame(1);

        Equal(2, P2.Graveyard.Creatures.Count());
        Equal(19, P2.Life);
      }

      [Fact]
      public void CannotCastWithOnly2Targets()
      {
        Hand(P1, "Cone Of Flame");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Juggernaut");

        RunGame(1);

        Equal(0, P2.Graveyard.Creatures.Count());
        Equal(20, P2.Life);
      }

      [Fact]
      public void CounteringConeTwiceShouldNotGetBug()
      {
        P1.Life = 1;
        Battlefield(P1, "Mountain", "Plains", "Plains", "Mountain", "Plains");
        Hand(P1, "Cone of Flame");

        Battlefield(P2, "Venom Sliver", "Diffusion Sliver", "Illusory Angel");

        RunGame(2);
      }
    }
  }
}