namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DevoutHarpist
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DestroyPacifism()
      {
        var pacifism = C("Pacifism");
        var dragon = C("Shivan Dragon");

        Hand(P1, pacifism);
        Battlefield(P1, "Plains");
        Battlefield(P2, dragon, "Devout Harpist");

        Exec(
          At(Step.FirstMain)
            .Cast(pacifism, target: dragon),
          At(Step.SecondMain, turn: 2)
            .Verify(() => Equal(Zone.Graveyard, C(pacifism).Zone))
          );
      }
    }
  }
}