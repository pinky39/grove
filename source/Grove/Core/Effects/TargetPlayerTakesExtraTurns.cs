namespace Grove.Core.Effects
{
  public class TargetPlayerTakesExtraTurns : Effect
  {
    public int Count = 1;

    public override void Resolve()
    {      
      Players.ScheduleExtraTurns(Target.Player(), Count);
    }
  }
}