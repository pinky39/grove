namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class StaffOfTheFlameMagus : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Staff of the Flame Magus")
                .ManaCost("{3}")
                .Type("Artifact")
                .Text("Whenever you cast a red spell or a Mountain enters the battlefield under your control, you gain 1 life.")
                .FlavorText("A symbol of passion in indifferent times.")
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever you cast a red spell or a Mountain enters the battlefield under your control, you gain 1 life.";

                    p.Trigger(new OnCastedSpell((ability, card) => card.HasColor(CardColor.Red) && card.Controller == ability.OwningCard.Controller));
                    p.Trigger(new OnZoneChanged(
                        to: Zone.Battlefield,
                        filter: (card, ability, game) => card.Is().Land && card.Type.Contains("Mountain") && card.Controller == ability.OwningCard.Controller
                        ));

                    p.Effect = () => new YouGainLife(1);

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
