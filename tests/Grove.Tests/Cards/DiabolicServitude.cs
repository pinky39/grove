namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class DiabolicServitude
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutCreatureToPlayDestroyCreature()
      {
        var servitude = C("Diabolic Servitude");
        var force = C("Verdant Force");
        var expunge = C("Expunge");

        Hand(P1, servitude);
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
        Graveyard(P1, force, "Grizzly Bears");

        Battlefield(P2, "Swamp", "Swamp", "Swamp");
        Hand(P2, expunge);

        RunGame(2);

        Equal(Zone.Graveyard, C(expunge).Zone);
        Equal(Zone.Exile, C(force).Zone);
        Equal(Zone.Hand, C(servitude).Zone);
      }
    }


    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PutCreatureToPlayDisenchant()
      {
        var servitude = C("Diabolic Servitude");
        var force = C("Verdant Force");
        var disenchant = C("Disenchant");

        Hand(P1, servitude);
        Graveyard(P1, force);
        Hand(P2, disenchant);

        Exec(
          At(Step.FirstMain)
            .Cast(servitude)
            .Target(force)
            .Verify(() => Equal(Zone.Battlefield, C(force).Zone)),
          At(Step.SecondMain)
            .Cast(disenchant, target: servitude)
            .Verify(() => Equal(Zone.Exile, C(force).Zone))
          );
      }

      [Fact]
      public void PutCreatureToPlayDestroyCreature()
      {
        var servitude = C("Diabolic Servitude");
        var force = C("Verdant Force");
        var expunge = C("Expunge");

        Hand(P1, servitude);
        Graveyard(P1, force);
        Hand(P2, expunge);

        Exec(
          At(Step.FirstMain)
            .Cast(servitude)
            .Target(force)
            .Verify(() => Equal(Zone.Battlefield, C(force).Zone)),
          At(Step.SecondMain)
            .Cast(expunge, target: force)
            .Verify(() =>
              {
                Equal(Zone.Hand, C(servitude).Zone);
                Equal(Zone.Exile, C(force).Zone);
              })
          );
      }
    }
  }
}