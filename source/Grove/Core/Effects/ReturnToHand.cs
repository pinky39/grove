namespace Grove.Effects
{
    using AI;
    using Infrastructure;

    public class ReturnToHand : Effect
    {
        private readonly int _discard;
        private readonly bool _returnOwningCard;
        private Zone _owningCardZone;
        private readonly DynParam<Card> _card;

        private ReturnToHand()
        {
        }

        public ReturnToHand(int discard = 0, bool returnOwningCard = false, EffectTag tag = EffectTag.Bounce)
        {
            _discard = discard;
            _returnOwningCard = returnOwningCard;

            SetTags(tag);
        }

        public ReturnToHand(DynParam<Card> card)
        {
            _card = card;

            RegisterDynamicParameters(card);
        }

        protected override void Initialize()
        {
            _owningCardZone = Source.OwningCard.Zone;
        }

        protected override void ResolveEffect()
        {
            if (_card != null)
            {
                _card.Value.PutToHand();

                return;
            }


            foreach (var target in ValidEffectTargets)
            {
                target.Card().PutToHand();
            }

            if (_returnOwningCard && ValidEffectTargets.None(x => x == Source.OwningCard))
            {
                Source.OwningCard.PutToHandFrom(_owningCardZone);
            }

            if (_discard > 0)
            {
                Enqueue(new Decisions.DiscardCards(
                  Target.Card().Controller,
                  p => p.Count = _discard));
            }
        }
    }
}