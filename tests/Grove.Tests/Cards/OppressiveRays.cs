namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class OppressiveRays
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void NotEnoughManaToAttack()
      {
        Hand(P1, "Oppressive Rays");
        Battlefield(P1, "Plains");        
        P1.Life = 2;

        Battlefield(P2, "Grizzly Bears", "Plains", "Plains");        

        RunGame(2);

        Equal(2, P1.Life);
      }

      [Fact]
      public void NotEnoughManaToBlock()
      {
        Hand(P1, "Oppressive Rays");
        Battlefield(P1, "Grizzly Bears", "Plains");

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears", "Plains", "Plains");

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void PayManaToAttack()
      {                
        Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Oppressive Rays"),
          "Plains", "Plains", "Plains");

        P2.Life = 2;
        RunGame(1);

        Equal(0, P2.Life);
        True(P1.Battlefield.Lands.All(x => x.IsTapped));
      }

      [Fact]
      public void PayManaToBlock()
      {        
        Battlefield(P1, "Juggernaut");

        P2.Life = 2;
        Battlefield(P2, C("Grizzly Bears").IsEnchantedWith("Oppressive Rays"), 
          "Plains", "Plains", "Plains");

        RunGame(1);

        Equal(2, P2.Life);
        True(P2.Battlefield.Lands.All(x => x.IsTapped));
      }

      [Fact]
      public void EnchantedCreatureCannotActivateAbility()
      {
        Battlefield(P1, "Juggernaut");

        P2.Life = 5;
        Battlefield(P2, C("Torch Fiend").IsEnchantedWith("Pacifism", "Oppressive Rays"), "Mountain");

        RunGame(1);

        Equal(0, P2.Life);
        Equal(1, P2.Battlefield.Creatures.Count());
      }

      [Fact]
      public void EnchantedCreatureCanActivateAbilityForPayingMana()
      {
        Battlefield(P1, "Juggernaut");

        P2.Life = 5;
        Battlefield(P2, C("Torch Fiend").IsEnchantedWith("Pacifism", "Oppressive Rays"), "Mountain", "Mountain", "Mountain", "Mountain");

        RunGame(1);

        Equal(5, P2.Life);
        Equal(0, P2.Battlefield.Creatures.Count());
      }

      [Fact]
      public void DoNotTryToActivateManaAbilityEndless()
      {
        Battlefield(P1, C("Elvish Mystic").IsEnchantedWith("Oppressive Rays"));

        RunGame(1);
      }

      [Fact]
      public void EnchantedJuggernautCanSkipAttack()
      {
        var juggernaut = C("Juggernaut").IsEnchantedWith("Oppressive Rays");

        P1.Life = 5;
        Battlefield(P1, "Plains", "Plains", "Plains", juggernaut);

        Battlefield(P2, "Juggernaut");

        RunGame(2);

        Equal(5, P1.Life);
      }
    }
  }
}
