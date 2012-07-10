namespace Grove.Core.Redirections
{
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