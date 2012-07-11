namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class OpalAcrolith : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Opal Acrolith")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent casts a creature spell, if Opal Acrolith is an enchantment, Opal Acrolith becomes a 2/4 Soldier creature.{EOL}{0}: Opal Acrolith becomes an enchantment.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "Whenever an opponent casts a creature spell, if Opal Acrolith is an enchantment, Opal Acrolith becomes a 2/4 Soldier creature.",
            C.Trigger<SpellWasCast>((t, _) => t.Filter =
              (ability, card) => ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<ChangeToCreature>((m, _) =>
                {
                  m.Power = 2;
                  m.Tougness = 4;
                  m.Type = "Creature - Soldier";
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true),
          C.ActivatedAbility(
            "{0}: Opal Acrolith becomes an enchantment.",
            C.Cost<TapOwnerPayMana>(),
            C.Effect<RemoveModifier>((e, _) => e.ModifierType = typeof (ChangeToCreature)),
            timing: Timings.BeforeDeath()
            )
        );
    }
  }
}