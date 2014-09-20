namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Momentum : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Momentum")
        .ManaCost("{2}{G}")
        .Type("Enchantment - Aura")
        .Text(
          "At the beginning of your upkeep, you may put a growth counter on Momentum.{EOL}Enchanted creature gets +1/+1 for each growth counter on Momentum.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              new IncreasePowerToughnessEqualToAttachedCounterCount(CounterType.Growth));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a growth counter on Momentum.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToSelf(() =>
                new AddCounters(() => new SimpleCounter(CounterType.Growth), 1));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}