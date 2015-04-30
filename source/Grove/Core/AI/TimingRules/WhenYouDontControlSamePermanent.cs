namespace Grove.AI.TimingRules
{
  using Grove.Infrastructure;

  public class WhenYouDontControlSamePermanent : TimingRule
  {
    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
    {
      return p.Controller.Battlefield.None(x => x.Name == p.Card.Name);
    }
  }
}