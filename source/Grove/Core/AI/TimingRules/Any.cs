namespace Grove.AI.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class Any : TimingRule
  {
    private readonly List<TimingRule> _rules;

    private Any() {}

    public Any(params TimingRule[] rules)
    {
      _rules = rules.ToList();
    }

    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      bool? result = null;
      foreach (var rule in _rules)
      {
        result = rule.ShouldPlay1(p);
        
        if (result == true)
          return true;

        if (result == null)
          return null;
      }

      return result;           
    }
        
    public override bool? ShouldPlay2(TimingRuleParameters p)
    {            
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