namespace Grove.Effects
{
  public class TargetPlayerGainsLife : Effect
  {
    private readonly int _amount;

    private TargetPlayerGainsLife() {}

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