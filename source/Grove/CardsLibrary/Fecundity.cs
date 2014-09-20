namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class Fecundity : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fecundity")
        .ManaCost("{2}{G}")
        .Type("Enchantment")
        .Text("Whenever a creature dies, that creature's controller may draw a card.")
        .FlavorText("Life is eternal. A lifetime is ephemeral.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature dies, that creature's controller may draw a card.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, g) => c.Is().Creature));

            p.Effect = () => new DrawCards(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChangedEvent>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}