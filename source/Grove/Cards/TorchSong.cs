namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Counters;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;
  using Gameplay.Triggers;

  public class TorchSong : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Torch Song")
        .ManaCost("{2}{R}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Torch Song.{EOL}{2}{R}, Sacrifice Torch Song: Torch Song deals X damage to target creature or player, where X is the number of verse counters on Torch Song.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a verse counter on Torch Song.";
            p.Trigger(new OnStepStart(step: Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
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
              amount: P(e => e.Source.OwningCard.Counters.GetValueOrDefault()));

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new DealDamage(pt => pt.Card.Counters.GetValueOrDefault()));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}