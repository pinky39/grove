namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class HiddenPredators : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Predators")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent controls a creature with power 4 or greater, if Hidden Predators is an enchantment, Hidden Predators becomes a 4/4 Beast creature.")
        .Abilities(
          TriggeredAbility(
            "When an opponent controls a creature with power 4 or greater, if Hidden Predators is an enchantment, Hidden Predators becomes a 4/4 Beast creature.",
            Trigger<OnEffectResolved>(t => t.Filter =
              (ability, game) =>
                {
                  if (ability.OwningCard.Is().Enchantment == false)
                    return false;

                  return ability.Controller.Opponent
                    .Battlefield.Creatures.Any(x => x.Power >= 4);
                }),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 4;
                  m.Toughness = 4;
                  m.Type = "Creature - Beast";
                  m.Colors = ManaColors.Green;
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}