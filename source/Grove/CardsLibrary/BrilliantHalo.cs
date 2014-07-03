namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class BrilliantHalo : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Brilliant Halo")
        .ManaCost("{1}{W}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature{EOL}Enchanted creature gets +1/+2.{EOL}When Brilliant Halo is put into a graveyard from the battlefield, return Brilliant Halo to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddPowerAndToughness(1, 2));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Brilliant Halo is put into a graveyard from the battlefield, return Brilliant Halo to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}