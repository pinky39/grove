namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class Oppression : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Oppression")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("Whenever a player casts a spell, that player discards a card.")
        .FlavorText("Do not presume to speak for yourself.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a player casts a spell, that player discards a card.";
            p.Trigger(new OnCastedSpell());
            p.Effect = () => new DiscardCards(1, P(e => e.TriggerMessage<AfterSpellWasPutOnStack>().Card.Controller));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}