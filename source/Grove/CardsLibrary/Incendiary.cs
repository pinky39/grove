namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Incendiary : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Incendiary")
        .ManaCost("{R}")
        .Type("Enchantment Aura")
        .Text("At the beginning of your upkeep, you may put a fuse counter on Incendiary.{EOL}When enchanted creature dies, Incendiary deals X damage to target creature or player, where X is the number of fuse counters on Incendiary.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectOrCostRankBy(c => c.Toughness.GetValueOrDefault(), ControlledBy.SpellOwner));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a fuse counter on Incendiary.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Fuse), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When enchanted creature dies, Incendiary deals X damage to target creature or player, where X is the number of fuse counters on Incendiary.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard,
              filter: (c, a, _) => c == a.OwningCard.AttachedTo));

            p.Effect = () => new DealDamageToTargets(P(e =>
              {
                return e.Source.OwningCard.CountersCount(CounterType.Fuse);
              }));
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            
            // we cannot obtain the number of counters since this executes
            // when counters are already removed, just put an estimate
            // here
            p.TargetingRule(new EffectDealDamage(4));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}