﻿namespace Grove.Tests.Cards
{
  using Core;
  using Infrastructure;
  using Xunit;

  public class Antagonism
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Deals2Damage()
      {
        var antagonism = C("Antagonism");
        var shock = C("Shock");
        
        Hand(P1, antagonism, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2),
          At(Step.SecondMain)
            .Cast(antagonism),
          At(Step.FirstMain, turn: 3)
            .Verify(() =>
              {
                Equal(20, P1.Life);
                Equal(16, P2.Life);
              })
          );
      }
    }
  }
}