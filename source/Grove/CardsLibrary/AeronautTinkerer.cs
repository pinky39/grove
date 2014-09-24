namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Modifiers;
    using Triggers;

    public class AeronautTinkerer : CardTemplateSource
    {
        private static Lifetime UntilControls()
        {
            return new OwnerControlsPermamentsLifetime(c => c.Is().Artifact);
        }

        private static Effect GetEffect()
        {
            return new ApplyModifiersToSelf(() =>
            {
                var modifier = new AddStaticAbility(Static.Flying);
                modifier.AddLifetime(UntilControls());
                return modifier;
            });
        }

        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Aeronaut Tinkerer")
                .ManaCost("{2}{U}")
                .Type("Creature — Human Artificer")
                .Text("Aeronaut Tinkerer has flying as long as you control an artifact.{I}(It can't be blocked except by creatures with flying or reach.){/I}")
                .FlavorText("\"All tinkerers have their heads in the clouds. I don't intend to stop there.\"")
                .Power(2)
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
