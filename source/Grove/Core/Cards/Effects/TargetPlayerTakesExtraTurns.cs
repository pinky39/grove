namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;

  public class TargetPlayerTakesExtraTurns : Effect
  {
    public int Count = 1;    

    protected override void ResolveEffect()
    {
      Players.ScheduleExtraTurns(Target().Player(), Count);
    }
  }
}