namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class PowerTaint : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Power Taint")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant enchantment}{EOL}At the beginning of the upkeep of enchanted enchantment's controller, that player loses 2 life unless he or she pays {2}.{EOL}Cycling {2}")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of the upkeep of enchanted enchantment's controller, that player loses 2 life unless he or she pays {2}.";

            p.Trigger(new OnStepStart(
              step: Step.Upkeep,
              passiveTurn: true,
              activeTurn: true)
              {
                Condition = (t, g) => t.OwningCard.AttachedTo.Controller.IsActive
              });

            p.Effect = () => new PayManaThen(2.Colorless(),
              effect: new ChangeLifeOfEnchantedPermanentsController(-2),
              parameters: new PayThen.Parameters()
              {
                ExecuteIfPaid = false,
              });

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}