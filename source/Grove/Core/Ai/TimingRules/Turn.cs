namespace Grove.Core.Ai.TimingRules
{
  public class Turn : TimingRule
  {
    public bool Active;
    public bool Passive;
    
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return (p.Controller.IsActive && Active) || (!p.Controller.IsActive && Passive);
    }
  }
}