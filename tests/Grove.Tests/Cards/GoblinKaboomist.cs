namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GoblinKaboomist
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void GetTokenThenActivateIt()
      {
        var kaboomist = C("Goblin Kaboomist");
        var bear = C("Grizzly Bears");

        Battlefield(P1, bear);
        Battlefield(P2, kaboomist, "Mountain");

        P2.Life = 2;

        Exec(
          At(Step.DeclareAttackers, turn: 3)
            .DeclareAttackers(bear),
          At(Step.SecondMain, turn: 3)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(bear).Zone);
                True(C(kaboomist).Zone == Zone.Graveyard || C(kaboomist).Zone == Zone.Battlefield);
              })
          );
      }
    }
  }
}