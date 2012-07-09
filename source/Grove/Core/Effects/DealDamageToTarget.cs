namespace Grove.Core.Effects
{
  using System;
  using Ai;
  using Modifiers;

  public class DealDamageToTarget : Effect, IDamageDealing
  {
    private Func<Effect, Value> _amount = delegate { return 0; };
    public bool GainLife = false;

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
      var damageSource = Source.OwningCard;
      var target = Target();

      var dealtDamage = target.DealDamage(damageSource, GetAmount().GetValue(X));

      if (GainLife)
        Controller.Life += dealtDamage;
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