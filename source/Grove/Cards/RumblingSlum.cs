namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class RumblingSlum : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Rumbling Slum")
        .ManaCost("{1}{R}{G}{G}")
        .Type("Creature - Elemental")
        .Text("At the beginning of your upkeep, Rumbling Slum deals 1 damage to each player.")
        .FlavorText(
          "The Orzhov contract the Izzet to animate slum districts and banish them to the wastes. The Gruul adopt them and send them back to the city for vengeance.")
        .Power(5)
        .Toughness(5)
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, Rumbling Slum deals 1 damage to each player.",
            Trigger<OnStepStart>(t => t.Step = Step.Untap),
            Effect<DealDamageToEach>(e => e.AmountPlayer = 1),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}