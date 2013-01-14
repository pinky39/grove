namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;


  public class VerdantForce : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Verdant Force")
        .ManaCost("{5}{G}{G}{G}")
        .Type("Creature - Elemental")
        .Text("At the beginning of each upkeep, put a 1/1 green Saproling creature token onto the battlefield.")
        .FlavorText(
          "Left to itself, nature overflows any container, overthrows any restriction, and overreaches any boundary.")
        .Power(7)
        .Toughness(7)
        .Abilities(
          TriggeredAbility(
            "At the beginning of each upkeep, put a 1/1 green Saproling creature token onto the battlefield.",
            Trigger<OnStepStart>(t =>
              {
                t.Step = Step.Upkeep;
                t.PassiveTurn = true;
                t.ActiveTurn = true;
              }),
            Effect<CreateTokens>(e => e.Tokens(
              Card
                .Named("Saproling Token")
                .FlavorText(
                  "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey.")
                .Power(1)
                .Toughness(1)
                .Type("Creature - Token - Saproling")
                .Colors(ManaColors.Green))),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}