namespace Grove.Effects
{
  public class TargetPlayerLoosesLife : Effect
  {
    private readonly int _amount;

    private TargetPlayerLoosesLife() {}

    public TargetPlayerLoosesLife(int amount)
    {
      _amount = amount;
    }

    protected override void ResolveEffect()
    {
      Target.Player().Life -= _amount;
    }
  }
}