namespace Grove.Gameplay.Damage
{
  using System;
  using Targeting;

  public class RedirectDamageToTarget : DamageRedirection
  {
    private readonly Func<DamageRedirection, ITarget> _target;

    private RedirectDamageToTarget() {}

    public RedirectDamageToTarget(Func<DamageRedirection, ITarget> target)
    {
      _target = target;
    }


    protected override bool Redirect(Damage damage)
    {
      _target(this).DealDamage(damage);

      return true;
    }
  }
}