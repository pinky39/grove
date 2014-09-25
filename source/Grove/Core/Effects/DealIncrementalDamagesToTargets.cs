namespace Grove.Effects
{
    using System.Linq;
    using AI;

    public class DealIncrementalDamagesToTargets : Effect
    {
        private readonly DynParam<int> _amount;

        private readonly int _damageIncrement;

        private DealIncrementalDamagesToTargets()
        {
        }

        public DealIncrementalDamagesToTargets(DynParam<int> startAmount, int damageIncrement = 0)
        {
            _amount = startAmount;
            _damageIncrement = damageIncrement;

            RegisterDynamicParameters(startAmount);
            SetTags(EffectTag.DealDamage);
        }

        public override int CalculatePlayerDamage(Player player)
        {
            return Targets.Effect.Any(x => x == player) ? _amount.Value + _damageIncrement * Targets.Effect.IndexOf(player) : 0;
        }

        public override int CalculateCreatureDamage(Card creature)
        {
            return Targets.Effect.Any(x => x == creature) ? _amount.Value + _damageIncrement * Targets.Effect.IndexOf(creature) : 0;
        }

        protected override void ResolveEffect()
        {
            var damage = _amount.Value;

            foreach (var target in Targets)
            {
                if (ValidEffectTargets.Contains(target))
                {
                    Source.OwningCard.DealDamageTo(
                        damage,
                        (IDamageable)target,
                        isCombat: false);
                }

                damage += _damageIncrement;
            }
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
