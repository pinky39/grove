namespace Grove.Core.Effects
{
  public class TargetPlayerTakesExtraTurns : Effect
  {
    public int Count = 1;

    protected override void ResolveEffect()
    {      
      Players.ScheduleExtraTurns(Target.Player(), Count);
    }
  }
}