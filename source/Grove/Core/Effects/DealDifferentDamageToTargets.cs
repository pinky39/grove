namespace Grove.Effects
{
  using System.Collections.Generic;
  using AI;

  public class DealDifferentDamageToTargets : Effect
  {
    private readonly List<int> _amounts = new List<int>();

    private DealDifferentDamageToTargets() {}

    public DealDifferentDamageToTargets(IEnumerable<int> amounts)
    {
      _amounts.AddRange(amounts);
      SetTags(EffectTag.DealDamage);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return CalculateTargetDamage(player);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return CalculateTargetDamage(creature);
    }

    private int CalculateTargetDamage(ITarget player)
    {
      var index = Targets.Effect.IndexOf(player);

      return index == -1
        ? 0
        : _amounts[index];
    }

    protected override void ResolveEffect()
    {
      for (var i = 0; i < _amounts.Count; i++)
      {
        var target = Targets.Effect[i];

        if (IsValid(target))
        {
          Source.OwningCard.DealDamageTo(
            _amounts[i],
            (IDamageable) target,
            isCombat: false);
        }
      }
    }
  }
}