namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class OpalGargoyle : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Gargoyle")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 2,
                toughness: 2,
                type: "Creature Gargoyle",
                colors: L(CardColor.White)),
              () => new AddStaticAbility(Static.Flying));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}