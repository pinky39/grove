namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class StabWound : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stab Wound")
        .ManaCost("{2}{B}")
        .Type("Enchantment — Aura")
        .Text(
          "Enchant creature{EOL}Enchanted creature gets -2/-2.{EOL}At the beginning of the upkeep of enchanted creature's controller, that player loses 2 life.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddPowerAndToughness(-2, -2));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());            
            p.TargetingRule(new EffectReduceToughness(2));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of the upkeep of enchanted creature's controller, that player loses 2 life.";

            p.Trigger(new OnStepStart(Step.Upkeep, activeTurn: true, passiveTurn: true)
              {
                Condition = (t, g) => t.OwningCard.IsAttached && t.OwningCard.AttachedTo.Controller.IsActive
              });

            p.Effect = () => new ChangeLifeOfEnchantedPermanentsController(-2);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}