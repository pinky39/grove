namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Messages;
  using Core.Triggers;
  using Core.Zones;

  public class Bereavement : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Bereavement")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("Whenever a green creature dies, its controller discards a card.")
        .FlavorText("Grief is as useless as love.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a green creature dies, its controller discards a card.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield, to: Zone.Graveyard,
              filter: (ability, card) => card.Is().Creature && card.HasColor(CardColor.Green)));

            p.Effect = () => new DiscardCards(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChanged>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}