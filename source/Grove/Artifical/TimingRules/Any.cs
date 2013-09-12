namespace Grove.Artifical.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;

  public class Any : TimingRule
  {
    private readonly List<TimingRule> _rules;

    private Any() {}

    public Any(params TimingRule[] rules)
    {
      _rules = rules.ToList();
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      // forces everything to be evaluated on 2nd pass
      
      bool? result = null;
      foreach (var rule in _rules)
      {
        result = rule.ShouldPlay2(p);
        
        if (result == true)
          return true;
      }

      return result;           
    }   

    public override void Initialize(Game game)
    {
      foreach (var rule in _rules)
      {
        rule.Initialize(game);
      }
    }
  }
}