namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class OnOpponentsTurn : TimingRule
  {
    private readonly Step _step;

    private OnOpponentsTurn() {}

    public OnOpponentsTurn(Step step)
    {
      _step = step;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return !p.Controller.IsActive && Turn.Step == _step;
    }
  }
}