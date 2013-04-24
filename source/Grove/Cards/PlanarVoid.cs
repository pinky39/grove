namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Zones;

  public class PlanarVoid : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Planar Void")
        .ManaCost("{B}")
        .Type("Enchantment")
        .Text("Whenever another card is put into a graveyard from anywhere, exile that card.")
        .FlavorText("'Planeswalking isn't about walking. It's about falling and screaming.'")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever another card is put into a graveyard from anywhere, exile that card.";

            p.Trigger(new OnZoneChanged(
              to: Zone.Graveyard,
              filter: delegate { return true; }));

            p.Effect = () => new ExileCard(P(e => e.TriggerMessage<ZoneChanged>().Card));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}