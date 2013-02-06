namespace Grove.Core.Effects
{
  using System;
  using System.Linq;
  using Modifiers;
  using Targeting;

  public class DealDamageToTargets : Effect
  {
    private readonly bool _gainLife;
    private readonly Func<Effect, int> _getAmount;

    private DealDamageToTargets() {}

    public DealDamageToTargets(Func<Effect, int> getAmount, bool gainLife = false)
    {
      _getAmount = getAmount;
      _gainLife = gainLife;
    }

    public DealDamageToTargets(Value amount, bool gainLife = false) : this(e => amount.GetValue(e.X), gainLife) {}


    public override int CalculatePlayerDamage(Player player)
    {
      return Targets.Effect.Any(x => x == player) ? _getAmount(this) : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return Targets.Effect.Any(x => x == creature) ? _getAmount(this) : 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var t in ValidEffectTargets)
      {
        var damage = new Damage(
          source: Source.OwningCard,
          amount: _getAmount(this),
          isCombat: false,
          changeTracker: Game.ChangeTracker);

        t.DealDamage(damage);

        if (_gainLife)
          Controller.Life += damage.Amount;
      }
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}