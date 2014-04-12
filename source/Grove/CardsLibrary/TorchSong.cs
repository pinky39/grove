namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class TorchSong : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Torch Song")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Torch Song.{EOL}{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Torch Song.";
            p.Trigger(new OnStepStart(step: Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Verse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.";
            p.Cost = new AggregateCost(
              new PayMana("{2}{R}".Parse(), ManaUsage.Abilities),
              new Sacrifice());
            p.Effect = () => new DealDamageToTargets(
              amount: P(e => e.Source.OwningCard.Counters));

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TargetingRule(new EffectDealDamage(pt => pt.Card.Counters));
            p.TimingRule(new WhenCardHasCounters(minCount: 2, onlyAtEot: false));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}