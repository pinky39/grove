namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class VolcanicFallout
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Deals2DamageToEachCreatureAndEachPlayer()
      {
        var armoddon = C("Trained Armodon");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        var fallout = C("Volcanic Fallout");

        Battlefield(P1, armoddon, bear1);
        Battlefield(P2, bear2);

        Hand(P1, fallout);

        Exec(
          At(Step.FirstMain)
            .Cast(fallout)
            .Verify(() =>
              {
                Equal(2, C(armoddon).Damage);
                Equal(Zone.Graveyard, C(bear1).Zone);
                Equal(Zone.Graveyard, C(bear2).Zone);
                Equal(18, P1.Life);
                Equal(18, P2.Life);
              })
          );
      }
    }
  }
}