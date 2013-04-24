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

  public class AngelicChorus : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Angelic Chorus")
        .ManaCost("{3}{W}{W}")
        .Type("Enchantment")
        .Text("Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.")
        .FlavorText("The very young and the very old know best the song the angels sing.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a creature enters the battlefield under your control, you gain life equal to its toughness.";
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              filter: (ability, card) => ability.OwningCard.Controller == card.Controller && card.Is().Creature));
            p.Effect = () => new ControllerGainsLife(
              amount: P(e => e.TriggerMessage<ZoneChanged>().Card.Toughness.GetValueOrDefault()));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}