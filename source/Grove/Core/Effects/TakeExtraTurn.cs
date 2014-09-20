namespace Grove.Effects
{
  public class TakeExtraTurn : Effect
  {
    protected override void ResolveEffect()
    {
      Players.ScheduleExtraTurns(Controller, 1);
    }
  }
}