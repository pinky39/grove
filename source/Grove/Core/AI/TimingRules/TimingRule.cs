namespace Grove.AI.TimingRules
{
  using System.Linq;
  using Grove.Infrastructure;

  public abstract class TimingRule : MachinePlayRule
  {
    public override bool Process(int pass, ActivationContext c)
    {
      if (pass == 1)
      {
        Process1(c);
        return false;
      }

      if (pass == 2)
      {
        Process2(c);
        return true;
      }

      return false;
    }


    public void Process1(ActivationContext c)
    {
      var p = new TimingRuleParameters(c.Card);
      var result = ShouldPlay1(p);

      if (result == false)
      {
        c.CancelActivation = true;
      }
      return;
    }

    public void Process2(ActivationContext c)
    {
      if (c.HasTargets == false)
      {
        // timing aplied before targeting, or
        // spell with no targets, evaluate just
        // one possiblility

        var p = new TimingRuleParameters(c.Card, x: c.X);
        var result = ShouldPlay2(p);

        if (result == false)
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

        var result = ShouldPlay2(p);
        if (result == false)
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

    public virtual bool? ShouldPlay2(TimingRuleParameters p)
    {
      return ShouldPlay1(p);
    }

    public virtual bool? ShouldPlay1(TimingRuleParameters p)
    {
      return null;
    }

    protected bool CanBeDestroyed(Card card, bool targetOnly = false, bool considerCombat = true)
    {
      if (considerCombat && Turn.Step == Step.DeclareBlockers && Stack.IsEmpty)
      {
        return Combat.CanBeDealtLeathalCombatDamage(card);
      }

      if (Stack.IsEmpty)
        return false;

      return Stack.CanBeDestroyedByTopSpell(card, targetOnly);
    }
  }
}