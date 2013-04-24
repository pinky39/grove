namespace Grove.Ai.TimingRules
{
  using Grove.Infrastructure;

  public class ThereCanBeOnlyOne : TimingRule
  {    
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.Battlefield.None(x => x.Name == p.Card.Name);
    }
  }
}