namespace Grove.Ai.TimingRules
{
  using Gameplay.States;

  public class DeclareBlockers : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.DeclareBlockers;
    }
  }
}