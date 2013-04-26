﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Messages;

  public class Oppression : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Oppression")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("Whenever a player casts a spell, that player discards a card.")
        .FlavorText("Do not presume to speak for yourself.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a player casts a spell, that player discards a card.";
            p.Trigger(new OnCastedSpell());
            p.Effect = () => new DiscardCards(1, P(e => e.TriggerMessage<PlayerHasCastASpell>().Card.Controller));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}