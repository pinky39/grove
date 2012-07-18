namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class RumblingSlum : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Rumbling Slum")
        .ManaCost("{1}{R}{G}{G}")
        .Type("Creature - Elemental")
        .Text("At the beginning of your upkeep, Rumbling Slum deals 1 damage to each player.")
        .FlavorText(
          "The Orzhov contract the Izzet to animate slum districts and banish them to the wastes. The Gruul adopt them and send them back to the city for vengeance.")
        .Power(5)
        .Toughness(5)
        .Timing(Timings.Creatures())
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, Rumbling Slum deals 1 damage to each player.",
            C.Trigger<AtBegginingOfStep>((t, _) => t.Step = Step.Untap),
            C.Effect<DealDamageToEach>(e => e.AmountPlayer = 1),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}