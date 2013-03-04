namespace Grove.Core.Effects
{
  using Targeting;

  public class TargetPlayerGainsLife : Effect
  {
    private readonly int _amount;

    public TargetPlayerGainsLife(int amount)
    {
      _amount = amount;
    }

    protected override void ResolveEffect()
    {
      Target.Player().Life += _amount;
    }
  }
}