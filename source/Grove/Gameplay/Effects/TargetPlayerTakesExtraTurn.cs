namespace Grove.Gameplay.Effects
{
  using Targeting;

  public class TargetPlayerTakesExtraTurn : Effect
  {
    protected override void ResolveEffect()
    {
      Players.ScheduleExtraTurns(Target.Player(), 1);
    }
  }
}