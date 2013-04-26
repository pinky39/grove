﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class HiddenPredators : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Predators")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent controls a creature with power 4 or greater, if Hidden Predators is an enchantment, Hidden Predators becomes a 4/4 Beast creature.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent controls a creature with power 4 or greater, if Hidden Predators is an enchantment, Hidden Predators becomes a 4/4 Beast creature.";
            p.Trigger(new OnEffectResolved(
              filter: (ability, game) =>
                {
                  if (ability.OwningCard.Is().Enchantment == false)
                    return false;

                  return ability.OwningCard.Controller.Opponent
                    .Battlefield.Creatures.Any(x => x.Power >= 4);
                }));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToCreature(
              power: 4,
              toughness: 4,
              type: "Creature Beast",
              colors: L(CardColor.Green)
              ));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}