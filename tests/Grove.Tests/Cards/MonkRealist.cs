namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MonkRealist
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DoNotPlayRealistWhenOpponentControlsNoEnchantments()
      {
        var realist = C("Monk Realist");

        Hand(P1, realist);
        Battlefield(P1, "Plains", "Plains");
        RunGame(1);


        Equal(Zone.Hand, C(realist).Zone);
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DestroyPacifism()
      {
        var pacifism = C("Pacifism");
        var bear = C("Grizzly Bears");
        var realist = C("Monk Realist");

        Hand(P1, pacifism);
        Hand(P2, realist);
        Battlefield(P1, "Plains", "Plains");
        Battlefield(P2, bear, "Plains", "Plains");

        Exec(
          At(Step.FirstMain)
            .Cast(pacifism, bear),
          At(Step.DeclareAttackers, turn: 2)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(pacifism).Zone);
                Equal(Zone.Battlefield, C(realist).Zone);
              })
          );
      }
    }
  }
}