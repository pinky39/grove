namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Counters;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class TorchSong : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Torch Song")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Torch Song.{EOL}{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, you may put a verse counter on Torch Song.",
            C.Trigger<AtBegginingOfStep>((t, _) => t.Step = Step.Upkeep),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddCounters>((m, c0) => { m.Counter = c0.Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          C.ActivatedAbility(
            "{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.",
            C.Cost<SacOwnerPayMana>((cost, _) => cost.Amount = "{2}{R}".ParseManaAmount()),
            C.Effect<DealDamageToTargets>(e => e.Amount = e.Source.OwningCard.Counters.GetValueOrDefault()),
            C.Validator(Validators.CreatureOrPlayer()),
            selectorAi: TargetSelectorAi.DealDamageSingleSelector(p => p.Source.Counters.GetValueOrDefault()),
            timing: All(Timings.InstantRemovalTarget())
        ));
    }
  }
}