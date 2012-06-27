namespace Grove.Core.Effects
{
  using System;
  using Ai;

  public class DealDamageToController : Effect, IDamageDealing
  {
    public Func<DealDamageToController, int> Amount = delegate { return 0; };

    public int PlayerDamage(Player player)
    {
      return player == Controller ? Amount(this) : 0;
    }

    public int CreatureDamage(Card creature)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      Controller.DealDamage(Source.OwningCard, Amount(this));
    }
  }
}