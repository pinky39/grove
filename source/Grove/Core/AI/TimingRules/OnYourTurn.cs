namespace Grove.AI.TimingRules
{
  public class OnYourTurn : TimingRule
  {
    private readonly Step _step;

    private OnYourTurn() {}

    public OnYourTurn(Step step)
    {
      _step = step;
    }

    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return p.Controller.IsActive && Turn.Step == _step && Stack.IsEmpty;
    }
  }
}