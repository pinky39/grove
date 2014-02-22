namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Triggers;

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
            p.TargetingRule(new EffectRankBy(c => -c.Score, ControlledBy.Opponent));
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

            p.Effect = () => new PayManaOrLooseLife(2, 2.Colorless(), 
             player: P((e, g) => e.Source.OwningCard.AttachedTo.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}