namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using AI.TargetingRules;
    using Costs;
    using Effects;
    using Modifiers;
    using Triggers;

    public class NightfireGiant : CardTemplateSource
    {
        private static Lifetime UntilControls()
        {
            return new OwnerControlsPermamentsLifetime(c => c.Is().OfType("Mountain"));
        }

        private static Effect GetEffect()
        {
            return new ApplyModifiersToSelf(
                () =>
                {
                    var modifier = new AddPowerAndToughness(1, 1);
                    modifier.AddLifetime(UntilControls());
                    return modifier;
                });
        }

        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Nightfire Giant")
                .ManaCost("{4}{B}")
                .Type("Creature — Zombie Giant")
                .Text("Nightfire Giant gets +1/+1 as long as you control a Mountain.{EOL}{4}{R}: Nightfire Giant deals 2 damage to target creature or player.")
                .FlavorText("Nightfire turns the greatest weakness of the undead into formidable strength.")
                .Power(4)
                .Toughness(3)
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
                    {
                        Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Mountain")) > 0
                    });

                    p.Effect = GetEffect;
                    p.UsesStack = false;
                })
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(
                        to: Zone.Battlefield,
                        filter: (card, ability, game) => card.Is().OfType("Mountain"))
                    {
                        Condition = (t, g) => t.OwningCard.Controller.Battlefield.Count(c => c.Is().OfType("Mountain")) == 1
                    });

                    p.Effect = GetEffect;

                    p.UsesStack = false;
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{4}{R}: Nightfire Giant deals 2 damage to target creature or player.";

                    p.Cost = new PayMana("{4}{R}".Parse(), ManaUsage.Abilities);

                    p.Effect = () => new DealDamageToTargets(2);

                    p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

                    p.TargetingRule(new EffectDealDamage(2));
                });
        }
    }
}
