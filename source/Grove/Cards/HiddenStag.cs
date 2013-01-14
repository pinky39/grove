namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class HiddenStag : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Stag")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent plays a land, if Hidden Stag is an enchantment, Hidden Stag becomes a 3/2 Elk Beast creature.{EOL}Whenever you play a land, if Hidden Stag is a creature, Hidden Stag becomes an enchantment.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "Whenever an opponent plays a land, if Hidden Stag is an enchantment, Hidden Stag becomes a 3/2 Elk Beast creature.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Land),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 3;
                  m.Toughness = 2;
                  m.Type = "Creature - Elk Beast";
                  m.Colors = ManaColors.Green;
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true),
          TriggeredAbility(
            "Whenever you play a land, if Hidden Stag is a creature, Hidden Stag becomes an enchantment.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.Controller == card.Controller && ability.OwningCard.Is().Creature && card.Is().Land),
            Effect<RemoveModifier>(e => e.ModifierType = typeof (ChangeToCreature)),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}