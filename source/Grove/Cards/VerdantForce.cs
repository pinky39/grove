namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class VerdantForce : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of each upkeep, put a 1/1 green Saproling creature token onto the battlefield.";

            p.Trigger(new OnStepStart(
              step: Step.Upkeep,
              passiveTurn: true,
              activeTurn: true
              ));

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Saproling Token")
                .FlavorText(
                  "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey.")
                .Power(1)
                .Toughness(1)
                .Type("Creature - Token - Saproling")
                .Colors(CardColor.Green));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}