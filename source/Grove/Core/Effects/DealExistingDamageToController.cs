namespace Grove.Core.Effects
{
  using System;

  public class DealExistingDamageToController : Effect
  {
    private readonly Func<Effect,Damage> _damage;
    
    public DealExistingDamageToController(Func<Effect, Damage> damage)
    {
      _damage = damage;
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return player == Controller ? _damage(this).Amount : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      Controller.DealDamage(_damage(this));
    }
  }
}