namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Modifiers;

  public class GraspOfDarkness : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Grasp of Darkness")
        .ManaCost("{B}{B}")
        .Type("Instant")
        .Text("Target creature gets -4/-4 until end of turn.")
        .FlavorText("On a world with five suns, night is compelled to become an aggressive force.")
        .Timing(Timings.TargetRemovalInstant())
        .Category(EffectCategories.ToughnessReduction)
        .Effect<ApplyModifiersToTarget>((e, c) => e.Modifiers(
          c.Modifier<AddPowerAndToughness>((m, _) =>
            {
              m.Power = -4;
              m.Toughness = -4;
            }, untilEndOfTurn: true)))
        .Targets(
          filter: TargetFilters.ReduceToughness(4),
          selectors: C.Selector(Selectors.Creature()));
    }
  }
}