namespace Grove.Gameplay.AI.TimingRules
{    
  public class AfterOpponentDeclaresBlockers : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return IsAfterOpponentDeclaresBlockers(p.Controller);
    }
  }
}