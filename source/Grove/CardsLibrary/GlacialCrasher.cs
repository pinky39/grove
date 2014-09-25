namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Modifiers;
    using Triggers;

    public class GlacialCrasher : CardTemplateSource
    {
        private static Lifetime UntilControls()
        {
            return new NoPermamentsOnBattlefieldLifetime(c => c.Is().OfType("Mountain"));
        }

        private static Effect GetEffect()
        {
            return new ApplyModifiersToSelf(
                () =>
                {
                    var modifier = new AddStaticAbility(Static.CannotAttack);
                    modifier.AddLifetime(UntilControls());
                    return modifier;
                });
        }

        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Glacial Crasher")
                .ManaCost("{4}{U}{U}")
                .Type("Creature — Elemental")
                .Text("{Trample}{I}(If this creature would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}{EOL}Glacial Crasher can't attack unless there is a Mountain on the battlefield.")
                .Power(5)
                .Toughness(5)
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
                    {
                        Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Mountain")) == 0
                    });

                    p.Effect = GetEffect;
                    p.UsesStack = false;
                })
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(
                        from: Zone.Battlefield,
                        filter: (card, ability, game) => card.Is().OfType("Mountain"))
                    {
                        Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Mountain")) == 0
                    });

                    p.Effect = GetEffect;

                    p.UsesStack = false;
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
