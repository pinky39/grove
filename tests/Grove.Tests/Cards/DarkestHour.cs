namespace Grove.Tests.Cards
{
  using Gameplay.Card.Characteristics;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class DarkestHour
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void BearsBecomeBlack()
      {
        var hour = C("Darkest Hour");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Hand(P1, hour);
        Battlefield(P1, bear1);
        Battlefield(P2, bear2);

        Exec(
          At(Step.FirstMain)
            .Cast(hour)
            .Verify(() =>
              {
                True(C(bear1).HasColor(CardColor.Black));
                True(C(bear2).HasColor(CardColor.Black));

                False(C(bear1).HasColor(CardColor.Green));
                False(C(bear2).HasColor(CardColor.Green));
              })
          );
      }
    }
  }
}