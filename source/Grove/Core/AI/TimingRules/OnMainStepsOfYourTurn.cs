namespace Grove.AI.TimingRules
{
  public class OnMainStepsOfYourTurn : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return (Turn.Step == Step.FirstMain || Turn.Step == Step.SecondMain) && p.Controller.IsActive && Stack.IsEmpty;
    }
  }
}