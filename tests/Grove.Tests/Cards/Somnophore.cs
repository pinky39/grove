namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class Somnophore
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TapWurmcoilEngine()
      {
        var engine = C("Wurmcoil Engine");

        Battlefield(P1, "Somnophore");
        Battlefield(P2, engine);

        RunGame(2);

        Equal(18, P2.Life);
        Equal(20, P1.Life);
        True(C(engine).IsTapped);
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void UntapNormalyIfDestroyed()
      {
        var engine = C("Wurmcoil Engine");
        var somnophore = C("Somnophore");
        var shock = C("Shock");

        Hand(P1, shock);
        Battlefield(P1, engine, "Mountain", "Mountain");
        Battlefield(P2, somnophore);

        Exec(
          At(Step.SecondMain, turn: 2)
            .Verify(() => True(C(engine).IsTapped)),
          At(Step.EndOfTurn, turn: 2)
            .Cast(shock, target: somnophore),
          At(Step.Upkeep, turn: 3)
            .Verify(() => False(C(engine).IsTapped))
          );
      }
    }
  }
}