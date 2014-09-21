namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;
    using Triggers;

    public class WallOfFrost : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Wall of Frost")
                .ManaCost("{1}{U}{U}")
                .Type("Creature — Wall")
                .Text("{Defender}{I}(This creature can't attack.){/I}{EOL}Whenever Wall of Frost blocks a creature, that creature doesn't untap during its controller's next untap step.")
                .FlavorText("\"I have seen countless petty warmongers gaze on it for a time before turning away.\"{EOL}—Sarlena, paladin of the Northern Verge")
                .Power(0)
                .Toughness(7)
                .SimpleAbilities(Static.Defender)
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever Wall of Frost blocks a creature, that creature doesn't untap during its controller's next untap step.";
                    p.Trigger(new OnBlock(blocks: true));

                    p.Effect = () => new ApplyModifiersToPermanents(
                        selector: (e, c) => c.BlockedBy(e.Source.OwningCard), 
                        controlledBy: ControlledBy.Opponent,
                        modifiers: () =>
                        {
                            var modifier = new AddStaticAbility(Static.DoesNotUntap);

                            modifier.AddLifetime(new EndOfStep(
                                Step.Untap,
                                l => l.Modifier.SourceCard.Controller.IsActive));

                            return modifier;
                        });

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
