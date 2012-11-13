namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class OpalCaryatid : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Caryatid")
        .ManaCost("{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Caryatid is an enchantment, Opal Caryatid becomes a 2/2 Soldier creature.")
        .Timing(Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "When an opponent casts a creature spell, if Opal Caryatid is an enchantment, Opal Caryatid becomes a 2/2 Soldier creature.",
            Trigger<SpellWasCast>(t => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature),
            Effect<Core.Cards.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                  m.Type = "Creature - Soldier";
                  m.Colors = ManaColors.White;
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}