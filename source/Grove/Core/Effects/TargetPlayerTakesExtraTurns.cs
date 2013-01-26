namespace Grove.Core.Effects
{
  using Grove.Core.Targeting;

  public class TargetPlayerTakesExtraTurns : Effect
  {
    public int Count = 1;    

    protected override void ResolveEffect()
    {
      Core.Players.ScheduleExtraTurns(Target().Player(), Count);
    }
  }
}