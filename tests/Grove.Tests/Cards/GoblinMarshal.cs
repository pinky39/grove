namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;  

  public class GoblinMarshal
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Get4Tokens()
      {
        var marshal = C("Goblin Marshal");

        Hand(P1, marshal);

        Exec(
          At(Step.FirstMain)
            .Cast(marshal),
          At(Step.SecondMain)
            .Verify(() => Equal(2, P1.Battlefield.Count(x => x.Is().Token))),
          At(Step.FirstMain, turn: 3)
            .Verify(() => Equal(4, P1.Battlefield.Count(x => x.Is().Token)))
            );
      }
    }
  }
}