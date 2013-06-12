namespace Grove.Gameplay.DamageHandling
{
  using Targeting;

  public class RedirectDamageFromTargetToTarget : DamageRedirection
  {
    private readonly ITarget _from;
    private readonly ITarget _to;

    private RedirectDamageFromTargetToTarget() {}

    public RedirectDamageFromTargetToTarget(ITarget from, ITarget to)
    {
      _from = from;
      _to = to;
    }

    protected override bool Redirect(Damage damage, ITarget target)
    {
      if (target == _from)
      {
        _to.ReceiveDamage(damage);
        return true;
      }

      return false;
    }
  }
}