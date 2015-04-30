namespace Grove.AI.TimingRules
{
  public class OnOpponentsTurn : TimingRule
  {
    private readonly Step _step;

    private OnOpponentsTurn() {}

    public OnOpponentsTurn(Step step)
    {
      _step = step;
    }

    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return Stack.IsEmpty && !p.Controller.IsActive && Turn.Step == _step;
    }
  }
}