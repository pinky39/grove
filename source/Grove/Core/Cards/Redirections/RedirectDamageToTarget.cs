namespace Grove.Core.Cards.Redirections
{
  using Grove.Core.Targeting;

  public class RedirectDamageToTarget : DamageRedirection
  {
    public ITarget Target { get; set; }

    protected override bool Redirect(Damage damage)
    {
      Target.DealDamage(damage);

      return true;
    }
  }
}