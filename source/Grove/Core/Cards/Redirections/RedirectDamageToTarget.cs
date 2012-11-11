namespace Grove.Core.Details.Cards.Redirections
{
  using Targeting;

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