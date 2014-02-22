using System.Collections.Generic;
using System.Linq;

namespace Grove.UserInterface
{
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
        new AutoPass {Step = Gameplay.Step.Untap, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.Upkeep, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.Draw, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.FirstMain, Pass = Pass.Passive},
        new AutoPass {Step = Gameplay.Step.BeginningOfCombat, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.DeclareAttackers, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.DeclareBlockers, Pass = Pass.Never},
        new AutoPass {Step = Gameplay.Step.CombatDamage, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.FirstStrikeCombatDamage, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.EndOfCombat, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.SecondMain, Pass = Pass.Passive},
        new AutoPass {Step = Gameplay.Step.EndOfTurn, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.Step.CleanUp, Pass = Pass.Always},
      };

    public static Configuration Default
    {
      get { return new Configuration(); }
    }

    public Pass GetAutoPassConfiguration(Gameplay.Step step)
    {
      return GetAutoPass(step).Pass;
    }

    public bool ShouldAutoPass(Gameplay.Step step, bool isActiveTurn, bool anyPlayerPlayedSomething)
    {
      if (anyPlayerPlayedSomething)
        return false;

      var config = GetAutoPass(step);

      if (config.Pass == Pass.Always)
        return true;

      return isActiveTurn
               ? config.Pass == Pass.Active
               : config.Pass == Pass.Passive;
    }

    public void ToggleAutoPass(Gameplay.Step step)
    {
      var config = GetAutoPass(step);
      config.Toggle();
    }

    private AutoPass GetAutoPass(Gameplay.Step step)
    {
      return _autoPassConfiguration.Single(x => x.Step == step);
    }

    private class AutoPass
    {
      public Pass Pass { get; set; }
      public Gameplay.Step Step { get; set; }

      public void Toggle()
      {
        Pass = (Pass) (((int) Pass + 1)%4);
      }
    }
  }
}