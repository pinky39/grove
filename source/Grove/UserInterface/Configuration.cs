namespace Grove.UserInterface
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Messages;
  using Infrastructure;

  public enum Pass
  {
    Always = 0,
    Passive = 1,
    Active = 2,
    Never = 3,
  }

  public class Configuration : ViewModelBase, IReceive<StepStarted>, IReceive<EffectPushedOnStack>
  {
    private readonly List<AutoPass> _autoPassConfiguration = new List<AutoPass>
      {
        new AutoPass {Step = Gameplay.States.Step.Untap, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.Upkeep, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.Draw, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.FirstMain, Pass = Pass.Passive},
        new AutoPass {Step = Gameplay.States.Step.BeginningOfCombat, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.DeclareAttackers, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.DeclareBlockers, Pass = Pass.Never},
        new AutoPass {Step = Gameplay.States.Step.CombatDamage, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.FirstStrikeCombatDamage, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.EndOfCombat, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.SecondMain, Pass = Pass.Passive},
        new AutoPass {Step = Gameplay.States.Step.EndOfTurn, Pass = Pass.Always},
        new AutoPass {Step = Gameplay.States.Step.CleanUp, Pass = Pass.Always},
      };

    private bool _hasAnyPlayerPlayedSpellOnThisStep = false;

    public void Receive(EffectPushedOnStack message)
    {
      _hasAnyPlayerPlayedSpellOnThisStep = true;
    }

    public void Receive(StepStarted message)
    {
      _hasAnyPlayerPlayedSpellOnThisStep = false;
    }

    public Pass GetAutoPassConfiguration(Gameplay.States.Step step)
    {
      return GetAutoPass(step).Pass;
    }

    public bool ShouldAutoPass()
    {
      if (_hasAnyPlayerPlayedSpellOnThisStep)
        return false;

      var step = CurrentGame.Turn.Step;
      var isActiveTurn = CurrentGame.Players.Active.IsHuman;

      var config = GetAutoPass(step);

      if (config.Pass == Pass.Always)
        return true;

      return isActiveTurn
        ? config.Pass == Pass.Active
        : config.Pass == Pass.Passive;
    }

    public void ToggleAutoPass(Gameplay.States.Step step)
    {
      var config = GetAutoPass(step);
      config.Toggle();
    }

    private AutoPass GetAutoPass(Gameplay.States.Step step)
    {
      return _autoPassConfiguration.Single(x => x.Step == step);
    }

    private class AutoPass
    {
      public Pass Pass { get; set; }
      public Gameplay.States.Step Step { get; set; }

      public void Toggle()
      {
        Pass = (Pass) (((int) Pass + 1)%4);
      }
    }
  }
}