namespace Grove.Core.Redirections
{
  public class RedirectDamageToTarget : DamageRedirection
  {
    public ITarget Target { get; set; }
    
    public override int RedirectDamage(Card damageDealer, int damageAmount, bool queryOnly)
    {      
      if (!queryOnly)
        Target.DealDamage(damageDealer, damageAmount);

      return 0;
    }
  }
}