namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;
  using ReturnToHand = Effects.ReturnToHand;

  public class Fortitude : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fortitude")
        .ManaCost("{1}{G}")
        .Type("Enchantment Aura")
        .Text(
          "Sacrifice a Forest: Regenerate enchanted creature.{EOL}When Fortitude is put into a graveyard from the battlefield, return Fortitude to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var ap = new ActivatedAbilityParameters
                  {
                    Text = "Sacrifice a Forest: Regenerate enchanted creature.",
                    Cost = new Sacrifice(),
                    Effect = () => new RegenerateOwner()
                  };

                ap.TargetSelector.AddCost(trg => trg
                  .Is.Card(x => x.Is("forest"), ControlledBy.SpellOwner)
                  .On.Battlefield());

                ap.TimingRule(new RegenerateSelfTimingRule());
                ap.TargetingRule(new CostSacrificeToRegenerate());

                return new AddActivatedAbility(new ActivatedAbility(ap));
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Fortitude is put into a graveyard from the battlefield, return Fortitude to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}