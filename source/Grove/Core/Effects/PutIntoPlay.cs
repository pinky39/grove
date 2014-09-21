namespace Grove.Effects
{
    public class PutIntoPlay : Effect
    {
        private readonly DynParam<bool> _tap;
        private readonly bool _putIntoBattlefield;
        private readonly DynParam<Card> _card;

        private PutIntoPlay() { }

        public PutIntoPlay(DynParam<bool> tap = null, bool putIntoBattlefield = false)
        {
            _putIntoBattlefield = putIntoBattlefield;

            _tap = tap ?? false;
            RegisterDynamicParameters(tap);
        }

        public PutIntoPlay(DynParam<Card> card)
        {
            _card = card;
            RegisterDynamicParameters(card);
        }

        protected override void ResolveEffect()
        {
            if (_card != null)
            {
                _card.Value.PutToBattlefield();

                return;
            }

            if (_tap.Value)
            {
                Source.OwningCard.Tap();
            }

            if (_putIntoBattlefield)
            {
                Source.OwningCard.PutToBattlefield();
            }
        }
    }
}