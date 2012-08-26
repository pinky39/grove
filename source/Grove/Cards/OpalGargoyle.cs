namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class OpalGargoyle : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Opal Gargoyle")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "When an opponent casts a creature spell, if Opal Gargoyle is an enchantment, Opal Gargoyle becomes a 2/2 Gargoyle creature with flying.",
            C.Trigger<SpellWasCast>((t, _) => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;                  
                  m.Type = "Creature - Gargoyle";
                  m.Colors = ManaColors.White;
                }),
              p.Builder.Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}