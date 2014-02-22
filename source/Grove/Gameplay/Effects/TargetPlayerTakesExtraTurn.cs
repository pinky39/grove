namespace Grove.Gameplay.Effects
{
  public class TargetPlayerTakesExtraTurn : Effect
  {
    protected override void ResolveEffect()
    {
      Players.ScheduleExtraTurns(Target.Player(), 1);
    }
  }
}