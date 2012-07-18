namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Humble : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Humble")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Target creature loses all abilities and becomes 0/1 until end of turn.")
        .FlavorText("'It is not your place to rule, Radiant. It may not even be mine.'{EOL}—Serra")
        .Timing(Timings.TargetRemovalInstant(combatOnly: true))
        .Category(EffectCategories.ToughnessReduction)
        .Effect<ApplyModifiersToTarget>(p => p.Effect.Modifiers(
          p.Builder.Modifier<DisableAbilities>(untilEndOfTurn: true),
          p.Builder.Modifier<SetPowerAndToughness>((m, _) =>
            {
              m.Power = 0;
              m.Tougness = 1;
            }, untilEndOfTurn: true)
          ))
        .Targets(
          filter: TargetFilters.Destroy(),
          effect: C.Selector(Selectors.Creature()));
    }
  }
}