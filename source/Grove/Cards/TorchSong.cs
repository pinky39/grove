namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Counters;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

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
        .Timing(Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you may put a verse counter on Torch Song.",
            Trigger<AtBegginingOfStep>(t => t.Step = Step.Upkeep),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddCounters>(m => { m.Counter = Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          ActivatedAbility(
            "{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.",
            Cost<SacOwnerPayMana>(cost => cost.Amount = "{2}{R}".ParseManaAmount()),
            Effect<DealDamageToTargets>(e => e.Amount = e.Source.OwningCard.Counters.GetValueOrDefault()),
            Validator(Validators.CreatureOrPlayer()),
            selectorAi: TargetSelectorAi.DealDamageSingleSelector(p => p.Source.Counters.GetValueOrDefault()),
            timing: All(Timings.InstantRemovalTarget())
        ));
    }
  }
}