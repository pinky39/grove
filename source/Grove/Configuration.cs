namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.States;

  public enum Pass
  {
    Always = 0,
    Passive = 1,
    Active = 2,
    Never = 3,
  }

  public class Configuration
  {
    private readonly List<AutoPass> _autoPassConfiguration = new List<AutoPass>
      {
        new AutoPass {Step = Step.Untap, Pass = Pass.Always},
        new AutoPass {Step = Step.Upkeep, Pass = Pass.Always},
        new AutoPass {Step = Step.Draw, Pass = Pass.Always},
        new AutoPass {Step = Step.FirstMain, Pass = Pass.Passive},
        new AutoPass {Step = Step.BeginningOfCombat, Pass = Pass.Always},
        new AutoPass {Step = Step.DeclareAttackers, Pass = Pass.Always},
        new AutoPass {Step = Step.DeclareBlockers, Pass = Pass.Never},
        new AutoPass {Step = Step.CombatDamage, Pass = Pass.Always},
        new AutoPass {Step = Step.FirstStrikeCombatDamage, Pass = Pass.Always},
        new AutoPass {Step = Step.EndOfCombat, Pass = Pass.Always},
        new AutoPass {Step = Step.SecondMain, Pass = Pass.Passive},
        new AutoPass {Step = Step.EndOfTurn, Pass = Pass.Always},
        new AutoPass {Step = Step.CleanUp, Pass = Pass.Always},
      };

    public Pass GetAutoPassConfiguration(Step step)
    {
      return GetAutoPass(step).Pass;
    }

    public bool ShouldAutoPass(Step step, bool isActiveTurn)
    {
      var config = GetAutoPass(step);

      if (config.Pass == Pass.Always)
        return true;

      return isActiveTurn
        ? config.Pass == Pass.Active
        : config.Pass == Pass.Passive;
    }

    public void ToggleAutoPass(Step step)
    {
      var config = GetAutoPass(step);
      config.Toggle();
    }

    private AutoPass GetAutoPass(Step step)
    {
      return _autoPassConfiguration.Single(x => x.Step == step);
    }

    private class AutoPass
    {
      public Pass Pass { get; set; }
      public Step Step { get; set; }

      public void Toggle()
      {
        Pass = (Pass) (((int) Pass + 1)%4);
      }
    }
  }
}