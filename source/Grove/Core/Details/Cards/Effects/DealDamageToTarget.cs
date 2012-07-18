namespace Grove.Core.Details.Cards.Effects
{
  using Ai;
  using Modifiers;
  using Targeting;

  public class DealDamageToTarget : Effect, IDamageDealing
  {
    public Value Amount = 0;
    public bool GainLife;

    public int PlayerDamage(Player player)
    {
      return player == Target() ? Amount.GetValue(X) : 0;
    }

    public int CreatureDamage(Card creature)
    {
      return creature == Target() ? Amount.GetValue(X) : 0;
    }

    protected override void ResolveEffect()
    {
      var damage = new Damage(
        source: Source.OwningCard,
        amount: Amount.GetValue(X),
        isCombat: false,
        changeTracker: Game.ChangeTracker);

      Target().DealDamage(damage);

      if (GainLife)
        Controller.Life += damage.Amount;
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}