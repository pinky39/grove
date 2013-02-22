namespace Grove.Core.Ai.TimingRules
{
  public class Turn : TimingRule
  {
    private readonly bool _active;
    private readonly bool _passive;

    private Turn() {}

    public Turn(bool active = false, bool passive = false)
    {
      _active = active;
      _passive = passive;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return (p.Controller.IsActive && _active) || (!p.Controller.IsActive && _passive);
    }
  }
}