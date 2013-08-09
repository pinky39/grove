namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

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
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a creature dies, that creature's controller may draw a card.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard,
              filter: (c, a, g) => c.Is().Creature));

            p.Effect = () => new DrawCards(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChanged>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}