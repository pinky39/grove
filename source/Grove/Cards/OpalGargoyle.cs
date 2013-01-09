namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class OpalGargoyle : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Gargoyle")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;                  
                  m.Type = "Creature - Gargoyle";
                  m.Colors = ManaColors.White;
                }),
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}