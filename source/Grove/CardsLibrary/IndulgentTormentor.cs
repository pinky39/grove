namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class IndulgentTormentor : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Indulgent Tormentor")
                .ManaCost("{3}{B}{B}")
                .Type("Creature — Demon")
                .Text("{Flying}{EOL}At the beginning of your upkeep, draw a card unless target opponent sacrifices a creature or pays 3 life.")
                .FlavorText("The promise of anguish is payment enough for services rendered.")
                .Power(5)
                .Toughness(3)
                .SimpleAbilities(Static.Flying)
                .TriggeredAbility(p =>
                {
                    p.Text = "At the beginning of your upkeep, draw a card unless target opponent sacrifices a creature or pays 3 life.";

                    p.Effect = () => new SacrificeCreatureOrPayLifeOrOpponentDrawsCard(3);

                    p.Trigger(new OnStepStart(Step.Upkeep));

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
