namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ArcheryTraining : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Archery Training")
        .ManaCost("{W}")
        .Type("Enchantment Aura")
        .Text(
          "At the beginning of your upkeep, you may put an arrow counter on Archery Training.{EOL}Enchanted creature has '{T}: This creature deals X damage to target attacking or blocking creature, where X is the number of arrow counters on Archery Training.'")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var ap = new ActivatedAbilityParameters
                  {
                    Text =
                      "{T}: This creature deals X damage to target attacking or blocking creature, where X is the number of arrow counters on Archery Training.",
                    Cost = new Tap(),
                    Effect = () => new DealDamageToTargets(P(e => e.Source.OwningCard.Attachments
                      .First(x => x.Name == "Archery Training")
                      .CountersCount(CounterType.Arrow)))
                  };

                ap.TargetSelector.AddEffect(trg => trg.Is.AttackerOrBlocker().On.Battlefield());
                ap.TimingRule(new OnStep(Step.DeclareBlockers));
                ap.TargetingRule(new EffectDealDamage(tp => tp.Card.Attachments
                  .First(x => x.Name == "Archery Training")
                  .CountersCount(CounterType.Arrow)));

                return new AddActivatedAbility(new ActivatedAbility(ap));
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put an arrow counter on Archery Training.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() => new AddCounters(() => new SimpleCounter(CounterType.Arrow), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}