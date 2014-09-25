namespace Grove.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Modifiers;

    public class CreateCopyOfTargetCreature : Effect
    {
        private readonly Action<Card, Game> _afterCopyCreated;
        private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
        private readonly Card _target;

        private CreateCopyOfTargetCreature() {}

        /// <summary>
        /// Creates copy of target creature. If taget param is null, Target property is used by TargetSelector
        /// </summary>
        /// <param name="target">Target creature to be copied</param>
        /// <param name="afterCopyCreated">Action applied to copy after creation. By default: PutOntoBattlefield</param>
        /// <param name="modifiers">Applyed to the copy</param>
        public CreateCopyOfTargetCreature(
            Card target = null,
            Action<Card, Game> afterCopyCreated = null,
            params CardModifierFactory[] modifiers)
        {
            _afterCopyCreated = afterCopyCreated ?? ((c, g) => c.PutToBattlefield());
            _target = target;
            _modifiers.AddRange(modifiers);
        }

        protected override void ResolveEffect()
        {
            if (_target == null && Target == null)
                return;

            var card = _target ?? Target.Card().Clone();
            card.Initialize(Controller, Game);

            var p = new ModifierParameters
            {
                SourceEffect = this,
                SourceCard = Source.OwningCard,
                X = X
            };

            foreach (var modifier in _modifiers.Select(factory => factory()))
            {
                card.AddModifier(modifier, p);
            }

            _afterCopyCreated(card, Game);
        }
    }
}
