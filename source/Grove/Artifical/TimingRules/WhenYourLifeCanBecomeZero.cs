namespace Grove.Artifical.TimingRules
{
  public class WhenYourLifeCanBecomeZero : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Stack.CanTopSpellReducePlayersLifeToZero(p.Controller);
    }
  }
}