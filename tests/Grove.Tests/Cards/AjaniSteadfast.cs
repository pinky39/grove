namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AjaniSteadfast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BearGets11()
      {
        var bear = C("Grizzly Bears");
        var ajani = C("Ajani Steadfast");
        
        Hand(P1, ajani);
        Battlefield(P1, bear, "Plains", "Plains", "Plains", "Plains");
        RunGame(1);
        
        Equal(17, P2.Life);
        Equal(23, P1.Life);
        Equal(5, C(ajani).Loyality);
      }

      [Fact]
      public void RedirectNonCombatDamage()
      {
        var ajani = C("Ajani Steadfast");

        Hand(P1, "Volcanic Hammer");
        Battlefield(P1, "Mountain", "Mountain");        
        Battlefield(P2, ajani.AddCounters(3, CounterType.Loyality));

        RunGame(1);

        Equal(Zone.Graveyard, C(ajani).Zone);
        Equal(20, P2.Life);
      }

      [Fact]
      public void AttackAjani()
      {
        var ajani = C("Ajani Steadfast");
        Battlefield(P1, "Trained Armodon");
        Battlefield(P2, ajani.AddCounters(3, CounterType.Loyality));

        RunGame(1);

        Equal(Zone.Graveyard, C(ajani).Zone);
        Equal(20, P2.Life);
      }

      [Fact]
      public void ActivateEmblemToPreventDamageFromDragons()
      {
        var ajani = C("Ajani Steadfast");
        Battlefield(P1, ajani.AddCounters(11, CounterType.Loyality));
        Battlefield(P2, "Shivan Dragon", "Shivan Dragon", "Shivan Dragon");

        RunGame(2);

        Equal(1, C(ajani).Loyality);
        Equal(20, P1.Life);
      }
    }
  }
}