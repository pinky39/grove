namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Triggers;

  public class VeiledSentry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veiled Sentry")
        .ManaCost("{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veiled Sentry is an enchantment, Veiled Sentry becomes an Illusion creature with power and toughness each equal to that spell's converted mana cost.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
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
                power: self => self.SourceEffect.TriggerMessage<AfterSpellWasPutOnStack>().Card.ConvertedCost,
                toughness: self => self.SourceEffect.TriggerMessage<AfterSpellWasPutOnStack>().Card.ConvertedCost,
                type: self => "Creature Illusion",
                colors: L(CardColor.Blue)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}