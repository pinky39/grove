namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Counters;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Targeting;
  using Core.Triggers;

  public class TorchSong : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Torch Song")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Torch Song.{EOL}{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.")
        .Cast(p=> p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you may put a verse counter on Torch Song.",
            Trigger<OnStepStart>(t => t.Step = Step.Upkeep),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddCounters>(m => { m.Counter = Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          ActivatedAbility(
            "{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.",
            Cost<PayMana, Sacrifice>(cost => cost.Amount = "{2}{R}".ParseMana()),
            Effect<DealDamageToTargets>(e => e.Amount = e.Source.OwningCard.Counters.GetValueOrDefault()),
            Target(Validators.CreatureOrPlayer(), Zones.Battlefield()),
            targetingAi: TargetingAi.DealDamageSingleSelector(p => p.Source.Counters.GetValueOrDefault()),
            timing: All(Timings.InstantRemovalTarget())
        ));
    }
  }
}