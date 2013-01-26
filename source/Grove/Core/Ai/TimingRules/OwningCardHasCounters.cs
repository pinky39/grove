namespace Grove.Core.Ai.TimingRules
{
  public class OwningCardHasCounters : TimingRule
  {
    public int MinCount;    

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Card.Counters >= MinCount && Turn.Step == Step.EndOfTurn && !p.Controller.IsActive)
        return true;

      return false;
    }
  }
}