namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class VeiledSentry : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Veiled Sentry")
        .ManaCost("{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veiled Sentry is an enchantment, Veiled Sentry becomes an Illusion creature with power and toughness each equal to that spell's converted mana cost.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veiled Sentry is an enchantment, Veiled Sentry becomes an Illusion creature with power and toughness each equal to that spell's converted mana cost.";
            p.Trigger(new OnCastedSpell(
              filter:
                (ability, card) =>
                  ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: self => self.SourceEffect.TriggerMessage<PlayerHasCastASpell>().Card.ConvertedCost,
                toughness: self => self.SourceEffect.TriggerMessage<PlayerHasCastASpell>().Card.ConvertedCost,
                type: self => "Creature Illusion",
                colors: L(CardColor.Blue)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}