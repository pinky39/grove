namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Fortitude : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
                    Effect = () => new Gameplay.Effects.Regenerate()
                  };

                ap.TargetSelector.AddCost(trg => trg
                  .Is.Card(x => x.Is("forest"), ControlledBy.SpellOwner)
                  .On.Battlefield());

                ap.TimingRule(new Artifical.TimingRules.Regenerate());
                ap.TargetingRule(new SacrificeToRegenerate());

                return new AddActivatedAbility(new ActivatedAbility(ap));
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Fortitude is put into a graveyard from the battlefield, return Fortitude to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new Gameplay.Effects.ReturnToHand(returnOwningCard: true);
          });
    }
  }
}