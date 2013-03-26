namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class OpalArchangel : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Archangel")
        .ManaCost("{4}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Archangel is an enchantment, Opal Archangel becomes a 5/5 Angel creature with flying and vigilance.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Archangel is an enchantment, Opal Archangel becomes a 5/5 Angel creature with flying and vigilance.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Core.Modifiers.ChangeToCreature(
                power: 5,
                toughness: 5,
                type: "Creature Angel",
                colors: ManaColors.White),
              () => new AddStaticAbility(Static.Flying),
              () => new AddStaticAbility(Static.Vigilance));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}