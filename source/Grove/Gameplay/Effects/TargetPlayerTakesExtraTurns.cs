namespace Grove.Gameplay.Effects
{
  using Targeting;

  public class TargetPlayerTakesExtraTurns : Effect
  {
    private readonly int _count;

    private TargetPlayerTakesExtraTurns() {}

    public TargetPlayerTakesExtraTurns(int count)
    {
      _count = count;
    }

    protected override void ResolveEffect()
    {
      Players.ScheduleExtraTurns(Target.Player(), _count);
    }
  }
}