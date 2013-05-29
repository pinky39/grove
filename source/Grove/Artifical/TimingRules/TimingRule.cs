namespace Grove.Artifical.TimingRules
{
  using System.Linq;
  using Gameplay.States;
  using Infrastructure;

  public abstract class TimingRule : MachinePlayRule
  {
    public override void Process(ActivationContext c)
    {
      if (c.HasTargets == false)
      {
        // timing aplied before targeting, or
        // spell with no targets, evaluate just
        // one possiblility

        var p = new TimingRuleParameters(c.Card);
        if (ShouldPlay(p) == false)
        {
          c.CancelActivation = true;
        }

        return;
      }

      // check each target timing, if ok keep 
      // the target otherwise remove it

      var targetsCombinations = c.TargetsCombinations().ToList();


      foreach (var targetsCombination in targetsCombinations)
      {
        var p = new TimingRuleParameters(c.Card, targetsCombination.Targets, targetsCombination.X);

        if (ShouldPlay(p) == false)
        {
          c.RemoveTargetCombination(targetsCombination);
        }
      }

      if (c.TargetsCombinations().None())
      {
        // if not targets are appropriate, cancel activation
        c.CancelActivation = true;
      }
    }

    public abstract bool ShouldPlay(TimingRuleParameters p);

    protected bool CanBeDestroyed(TimingRuleParameters p, bool targetOnly = false, bool considerCombat = true)
    {
      if (considerCombat && Turn.Step == Step.DeclareBlockers)
      {
        return Combat.CanBeDealtLeathalCombatDamage(p.Card);
      }

      if (Stack.IsEmpty)
        return false;

      return Stack.CanBeDestroyedByTopSpell(p.Card, targetOnly);
    }
  }
}