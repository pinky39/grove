namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Modifiers;
    using Triggers;

    public class ScrapyardMongrel : CardTemplateSource
    {
        private static Lifetime UntilControls()
        {
            return new OwnerControlsPermamentsLifetime(c => c.Is().Artifact);
        }

        private static Effect GetEffect()
        {
            return new ApplyModifiersToSelf(
                () =>
                {
                    var modifier = new AddPowerAndToughness(2, 0);
                    modifier.AddLifetime(UntilControls());
                    return modifier;
                },
                () =>
                {
                    var modifier = new AddStaticAbility(Static.Trample);
                    modifier.AddLifetime(UntilControls());
                    return modifier;
                });
        }

        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Scrapyard Mongrel")
                .ManaCost("{3}{R}")
                .Type("Creature — Hound")
                .Text("As long as you control an artifact, Scrapyard Mongrel gets +2/+0 and has trample.{I}(If it would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}")
                .FlavorText("Trespassers are welcome to try.")
                .Power(3)
                .Toughness(3)
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
                    {
                        Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().Artifact) > 0
                    });

                    p.Effect = GetEffect;
                    p.UsesStack = false;
                })
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(
                        to: Zone.Battlefield,
                        filter: (card, ability, game) => card.Is().Artifact)
                    {
                        Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().Artifact) == 1
                    });

                    p.Effect = GetEffect;

                    p.UsesStack = false;
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
