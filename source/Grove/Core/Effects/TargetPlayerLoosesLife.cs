namespace Grove.Effects
{
    using System.Linq;

    public class TargetPlayerLoosesLife : Effect
    {
        private readonly DynParam<int> _amount;

        private TargetPlayerLoosesLife() { }

        public TargetPlayerLoosesLife(DynParam<int> amount)
        {
            _amount = amount;

            RegisterDynamicParameters(amount);
        }

        public override int CalculatePlayerDamage(Player player)
        {
            return Targets.Effect.Any(x => x == player) ? _amount.Value : 0;
        }

        public override int CalculateCreatureDamage(Card creature)
        {
            return 0;
        }

        protected override void ResolveEffect()
        {
            Target.Player().Life -= _amount.Value;
        }
    }
}