namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class YawgmothsEdict
  {
    public class Predefined: PredefinedScenario
    {
      [Fact]
      public void YouGain1LifeOpponentLooses1Life()
      {
        var knight = C("White Knight");
        
        Battlefield(P1, "Yawgmoth's Edict");        
        Hand(P2, knight);

        Exec(
          At(Step.FirstMain, turn: 2)
            .Cast(knight)
            .Verify(() =>
              {
                Equal(19, P2.Life);
                Equal(21, P1.Life);
              })
          );
      }
    }
  }
}