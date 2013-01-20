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

  public class HiddenGuerrillas : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Guerrillas")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text("When an opponent casts an artifact spell, if Hidden Guerrillas is an enchantment, Hidden Guerrillas becomes a 5/3 Soldier creature with trample.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "When an opponent casts an artifact spell, if Hidden Guerrillas is an enchantment, Hidden Guerrillas becomes a 5/3 Soldier creature with trample.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Artifact),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 5;
                  m.Toughness = 3;
                  m.Type = "Creature - Soldier";
                  m.Colors = ManaColors.Green;                                    
                }),
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Trample)
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}