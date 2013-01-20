namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class HiddenAncients : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Ancients")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.")
        .FlavorText("The only alert the invaders had was the rustling of leaves on a day without wind.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Enchantment),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 5;
                  m.Toughness = 5;
                  m.Type = "Creature - Treefolk";
                  m.Colors = ManaColors.Green;
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}