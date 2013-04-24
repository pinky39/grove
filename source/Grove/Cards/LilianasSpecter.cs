namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class LilianasSpecter : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Liliana's Specter")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Liliana's Specter enters the battlefield, each opponent discards a card.")
        .FlavorText("'The finest minions know what I need without me ever saying a thing.'")
        .Power(2)
        .Toughness(1)
        .Cast(p => p.TimingRule(new FirstMain()))
        .StaticAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Liliana's Specter enters the battlefield, each opponent discards a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
          });
    }
  }
}