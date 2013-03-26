namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Messages;
  using Core.Triggers;

  public class Opression : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Opression")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("Whenever a player casts a spell, that player discards a card.")
        .FlavorText("'Do not presume to speak for yourself.'{EOL}—Gix, to Xantcha")
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