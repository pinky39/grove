namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class OnStep : TimingRule
  {
    private readonly Step _step;

    private OnStep() {}

    public OnStep(Step step)
    {
      _step = step;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == _step;
    }
  }
}