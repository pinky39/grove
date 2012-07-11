namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using Ai;
  using Modifiers;
  using Targeting;

  public class DealDamageToTarget : Effect, IDamageDealing
  {
    public bool GainLife;
    private Func<Effect, Value> _amount = delegate { return 0; };

    public int PlayerDamage(Player player)
    {
      return player == Target() ? GetAmount().GetValue(X) : 0;
    }

    public int CreatureDamage(Card creature)
    {
      return creature == Target() ? GetAmount().GetValue(X) : 0;
    }

    public void SetAmount(Value amount)
    {
      _amount = delegate { return amount; };
    }

    public void SetAmount(Func<Effect, Value> amount)
    {
      _amount = amount;
    }

    protected override void ResolveEffect()
    {
      var damage = new Damage(
        source: Source.OwningCard,
        amount: GetAmount().GetValue(X),
        isCombat: false,
        changeTracker: Game.ChangeTracker);

      Target().DealDamage(damage);

      if (GainLife)
        Controller.Life += damage.Amount;
    }

    private Value GetAmount()
    {
      return _amount(this);
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}