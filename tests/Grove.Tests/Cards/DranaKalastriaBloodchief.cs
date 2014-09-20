namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class DranaKalastriaBloodchief
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ActivateDrana()
      {
        Battlefield(P1, "Drana, Kalastria Bloodchief", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Grizzly Bears", "Llanowar Elves");

        RunGame(maxTurnCount: 2);

        Equal(2, P2.Graveyard.Count());
      }

      [Fact]
      public void ActivateDranaBeforeCombat()
      {
        var blade = C("Doom Blade");
        var armodon = C("Trained Armodon");

        Hand(P1, blade);
        Battlefield(P1, "Drana, Kalastria Bloodchief", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, armodon, "Forest", "Forest", "Forest", "Mountain");

        RunGame(maxTurnCount: 2);

        Equal(Zone.Hand, C(blade).Zone);
        Equal(Zone.Graveyard, C(armodon).Zone);
        Equal(13, P2.Life);
      }

      [Fact]
      public void PreferDranaBeforeGrasp()
      {
        Battlefield(P1, "Grizzly Bears");
        Battlefield(P2, "Drana, Kalastria Bloodchief", "Swamp", "Swamp", "Swamp", "Swamp");
        Hand(P2, "Grasp of Darkness");

        RunGame(maxTurnCount: 2);

        Equal(2, P2.Hand.Count());
        Equal(0, P1.Battlefield.Count());
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void LegendaryRule1()
      {
        var drana = C("Drana, Kalastria Bloodchief");

        Hand(P1, drana);
        Battlefield(P2, "Drana, Kalastria Bloodchief");

        Exec(
          At(Step.FirstMain)
            .Cast(drana)
            .Verify(() =>
              {
                Equal(0, P1.Battlefield.Count());
                Equal(0, P2.Battlefield.Count());
              }));
      }

      [Fact]
      public void LegendaryRule2()
      {
        var drana = C("Drana, Kalastria Bloodchief");

        Hand(P1, drana);
        Battlefield(P1, "Drana, Kalastria Bloodchief");

        Exec(
          At(Step.FirstMain)
            .Cast(drana)
            .Verify(() => Equal(0, P1.Battlefield.Count())));
      }

      [Fact]
      public void XActivationCost()
      {
        var bear = C("Grizzly Bears");
        var drana = C("Drana, Kalastria Bloodchief");

        Battlefield(P1, drana);
        Battlefield(P2, bear);

        Exec(
          At(Step.FirstMain)
            .Activate(drana, x: 1, target: bear)
            .Verify(() =>
              {
                Equal(1, C(bear).Toughness);
                Equal(5, C(drana).Power);
              }),
          At(Step.FirstMain, turn: 2)
            .Verify(() => Equal(4, C(drana).Power))
          );
      }
    }
  }
}