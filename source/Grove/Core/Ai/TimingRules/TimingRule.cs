namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;

  public abstract class TimingRule : MachinePlayRule
  {
    public override void Process(ActivationContext c)
    {            
      if (c.Targets.Count == 0)
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
      
      var targets = c.Targets.ToList();      
      foreach (var target in targets)
      {
        var p = new TimingRuleParameters(c.Card, target);

        if (ShouldPlay(p) == false)
        {
          c.Targets.Remove(target);
        }
      }

      if (c.Targets.Count == 0)
      {
        // if not targets are appropriate, cancel activation
        c.CancelActivation = true;
      }            
    }

    public abstract bool ShouldPlay(TimingRuleParameters p);
  }
}