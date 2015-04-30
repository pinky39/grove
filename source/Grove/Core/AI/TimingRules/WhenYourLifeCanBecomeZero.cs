namespace Grove.AI.TimingRules
{
  public class WhenYourLifeCanBecomeZero : TimingRule
  {
    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
    {
      return Stack.CanTopSpellReducePlayersLifeToZero(p.Controller);
    }
  }
}