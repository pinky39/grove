namespace Grove.Core.Effects
{
  using System;
  using Ai;

  public class DealExistingDamageToController : Effect, IDamageDealing
  {
    public Func<Effect, Damage> Damage { get; set; }
    
    public int PlayerDamage(Player player)
    {
      return player == Controller ? Damage(this).Amount : 0;
    }

    public int CreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      Controller.DealDamage(Damage(this));
    }
  }
}