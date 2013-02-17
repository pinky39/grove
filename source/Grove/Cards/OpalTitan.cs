namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Messages;
  using Core.Modifiers;
  using Core.Triggers;

  public class OpalTitan : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Titan")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Core.Modifiers.ChangeToCreature(
                power: 4,
                toughness: 4,
                type: "Creature Giant",
                colors: ManaColors.White),
              () => new AddProtectionFromColors(m =>
                m.SourceEffect.TriggerMessage<PlayerHasCastASpell>().Card.Colors));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}