namespace Grove.Core.Effects
{
  using System.Linq;
  using Modifiers;
  using Targeting;

  public class DealDamageToTargets : Effect
  {
    private readonly Value _amount;
    private readonly bool _gainLife;

    private DealDamageToTargets() {}

    public DealDamageToTargets(Value amount, bool gainLife = false)
    {
      _amount = amount;
      _gainLife = gainLife;
    }


    public override int CalculatePlayerDamage(Player player)
    {
      return Targets.Effect.Any(x => x == player) ? _amount.GetValue(X) : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return Targets.Effect.Any(x => x == creature) ? _amount.GetValue(X) : 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var t in ValidEffectTargets)
      {
        var damage = new Damage(
          source: Source.OwningCard,
          amount: _amount.GetValue(X),
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