
namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class BoonweaverGiant : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Boonweaver Giant")
                .ManaCost("{6}{W}")
                .Type("Creature — Giant Monk")
                .Text("When Boonweaver Giant enters the battlefield, you may search your graveyard, hand, and/or library for an Aura card and put it onto the battlefield attached to Boonweaver Giant. If you search your library this way, shuffle it.")
                .Power(4)
                .Toughness(4)
                .TriggeredAbility(p =>
                {
                    p.Text = "When Boonweaver Giant enters the battlefield, you may search your graveyard, hand, and/or library for an Aura card and put it onto the battlefield attached to Boonweaver Giant. If you search your library this way, shuffle it.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new ChangeZoneForSelectedCard(
                            zoneTo: Zone.Battlefield,
                            minCount: 0,
                            maxCount: 1,
                            validator: c => c.Is().Aura,
                            text: "Search your #0 for an Aura card.",
                            aurasNeedTarget: false);
//                            afterPutToZone: (card, effect) => card.EnchantWithoutPayingCost(effect.Source.OwningCard));
                });
        }
    }
}
