namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class OpalAcrolith : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Acrolith")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent casts a creature spell, if Opal Acrolith is an enchantment, Opal Acrolith becomes a 2/4 Soldier creature.{EOL}{0}: Opal Acrolith becomes an enchantment.")
        .Cast(p => p.Timing = Timings.SecondMain())          
        .Abilities(
          TriggeredAbility(
            "Whenever an opponent casts a creature spell, if Opal Acrolith is an enchantment, Opal Acrolith becomes a 2/4 Soldier creature.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 4;
                  m.Type = "Creature - Soldier";
                  m.Colors = ManaColors.White;
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true),
          ActivatedAbility(
            "{0}: Opal Acrolith becomes an enchantment.",
            Cost<PayMana>(c => c.Amount = ManaAmount.Zero),
            Effect<RemoveModifier>(e => e.ModifierType = typeof (ChangeToCreature)),
            timing: All(Timings.Has(c => c.Is().Creature), Timings.BeforeDeath())
            )
        );
    }
  }
}