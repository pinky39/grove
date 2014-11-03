namespace Grove.Effects
{
  using System.Linq;

  public class DealDamageToAndTapTargets : Effect
  {
    private readonly int _amount;

    private DealDamageToAndTapTargets() {}

    public DealDamageToAndTapTargets(int amount)
    {
      _amount = amount;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return Targets.Effect.Any(x => x == creature) ? _amount : 0;
    }

    protected override void ResolveEffect()
    {
      var targets = ValidEffectTargets.ToList();

      Source.OwningCard.DealDamageTo(
          _amount,
          (IDamageable)targets[0],
          isCombat: false);

      targets[1].Card().Tap();
    }
  }
}
