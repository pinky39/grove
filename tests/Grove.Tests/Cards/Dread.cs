namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Dread
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void KillAttackers()
      {
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Battlefield(P1, bear1, bear2);
        Battlefield(P2, "Dread");

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear1, bear2),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(bear1).Zone);
                Equal(Zone.Graveyard, C(bear2).Zone);
              })
          );
      }

      [Fact]
      public void KillRumblingSlum()
      {
        var slum = C("Rumbling Slum");

        Battlefield(P1, slum);
        Battlefield(P2, "Dread");

        Exec(
          At(Step.FirstMain)
            .Verify(() => Equal(Zone.Graveyard, C(slum).Zone))
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DreadAbilityTriggersTwiceGameEndsBug()
      {
        var engine = C("Wurmcoil Engine").IsEquipedWith("Sword of Feast and Famine");
        var student = C("Student of Warfare");

        Battlefield(P1, engine, student, "Plains", "Plains");
        Battlefield(P2, "Dread");

        Exec(
          At(Step.FirstMain)
            .Activate(student)
            .Activate(student),
          At(Step.DeclareAttackers)
            .DeclareAttackers(engine),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(engine).Zone);
                Equal(6, P1.Battlefield.Count());
                False(Game.IsFinished);
                Equal(12, P2.Life);
              })
          );
      }
    }
  }
}