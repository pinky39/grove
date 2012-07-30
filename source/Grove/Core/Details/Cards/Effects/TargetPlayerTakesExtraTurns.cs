namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class TargetPlayerTakesExtraTurns : Effect
  {
    public int Count = 1;

    public override bool NeedsTargets { get { return true; } }

    protected override void ResolveEffect()
    {
      Players.ScheduleExtraTurns(Target().Player(), Count);
    }
  }
}