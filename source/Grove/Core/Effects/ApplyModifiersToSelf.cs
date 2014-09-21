namespace Grove.Effects
{
    using System.Collections.Generic;
    using System.Linq;
    using Modifiers;

    public class ApplyModifiersToSelf : Effect
    {
        private readonly List<CardModifierFactory> _selfModifiers = new List<CardModifierFactory>();
        private readonly bool _toAttachedTo;
        private readonly bool _applyInAnyZone;

        private ApplyModifiersToSelf() { }

        public ApplyModifiersToSelf(params CardModifierFactory[] modifiers) : this(false, false, modifiers) { }

        public ApplyModifiersToSelf(bool toAttachedTo = false, bool applyInAnyZone = false, params CardModifierFactory[] modifiers)
        {
            _toAttachedTo = toAttachedTo;
            _applyInAnyZone = applyInAnyZone;
            _selfModifiers.AddRange(modifiers);
        }

        public override bool TargetsEffectSource { get { return true; } }

        public override int CalculateToughnessReduction(Card card)
        {
            return card == Source.OwningCard ? ToughnessReduction.GetValue(X) : 0;
        }

        protected override void ResolveEffect()
        {
            var target = _toAttachedTo ? Source.OwningCard.AttachedTo : Source.OwningCard;

            var p = new ModifierParameters
            {
                SourceEffect = this,
                SourceCard = Source.OwningCard,
                X = X
            };

            if (_applyInAnyZone || Source.OwningCard.Zone == Zone.Battlefield)
            {
                foreach (var modifier in _selfModifiers.Select(modifierFactory => modifierFactory()))
                {
                    target.AddModifier(modifier, p);
                }
            }
        }
    }
}