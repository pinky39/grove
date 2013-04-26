namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Zones;

  public class Fecundity : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
              filter: (ability, card) => card.Is().Creature));

            p.Effect = () => new DrawCards(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChanged>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}