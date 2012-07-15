namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class MonkRealist : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Monk Realist")
        .ManaCost("{1}{W}")
        .Type("Creature Human Monk Cleric")
        .Text("When Monk Realist enters the battlefield, destroy target enchantment.")
        .FlavorText("We plant the seeds of doubt to harvest the crop of wisdom.")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.FirstMain())
        .Abilities(
          C.TriggeredAbility(
            "When Monk Realist enters the battlefield, destroy target enchantment.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<DestroyTargetPermanent>(),
            C.Selector(Selectors.Permanent(card => card.Is().Enchantment)),
            targetFilter: TargetFilters.OrderByDescendingScore(),
            category: EffectCategories.Destruction)
        );
    }
  }
}