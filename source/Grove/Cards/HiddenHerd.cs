namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class HiddenHerd : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Herd")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent plays a nonbasic land, if Hidden Herd is an enchantment, Hidden Herd becomes a 3/3 Beast creature.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "When an opponent plays a nonbasic land, if Hidden Herd is an enchantment, Hidden Herd becomes a 3/3 Beast creature.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().NonBasicLand),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 3;
                  m.Toughness = 3;
                  m.Type = "Creature - Beast";
                  m.Colors = ManaColors.Green;
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}