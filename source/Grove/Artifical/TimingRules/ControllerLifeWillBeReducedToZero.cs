namespace Grove.Artifical.TimingRules
{
  public class ControllerLifeWillBeReducedToZero : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Stack.CanTopSpellReducePlayersLifeToZero(p.Controller);
    }
  }
}