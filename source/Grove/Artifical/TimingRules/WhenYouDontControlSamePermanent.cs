namespace Grove.Artifical.TimingRules
{
  using Infrastructure;

  public class WhenYouDontControlSamePermanent : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.Battlefield.None(x => x.Name == p.Card.Name);
    }
  }
}