namespace Grove.Ai.TimingRules
{
  public class ControllerLifeWillBeReducedToZero : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {      
      return Stack.CanTopSpellReducePlayersLifeToZero(p.Controller);
    }
  }
}