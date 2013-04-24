namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;

  public class OpalCaryatid : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Caryatid")
        .ManaCost("{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Caryatid is an enchantment, Opal Caryatid becomes a 2/2 Soldier creature.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Caryatid is an enchantment, Opal Caryatid becomes a 2/2 Soldier creature.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature));
            p.Effect = () => new ApplyModifiersToSelf(() => new Gameplay.Modifiers.ChangeToCreature(
              power: 2,
              toughness: 2,
              type: "Creature Soldier",
              colors: L(CardColor.White)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}