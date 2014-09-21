namespace Grove.Effects
{
    public class YourLifeBecomesEqual : Effect
    {
        private readonly DynParam<int> _amount;
        private Player _you;

        private YourLifeBecomesEqual()
        {
        }

        public YourLifeBecomesEqual(DynParam<int> amount)
        {
          _amount = amount;

          RegisterDynamicParameters(amount);
        }

        protected override void Initialize()
        {
          _you = Source.OwningCard.Controller;
        }

        protected override void ResolveEffect()
        {
          _you.Life = _amount.Value;
        }
    }
}
