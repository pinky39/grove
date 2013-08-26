namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Launch : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Launch")
        .ManaCost("{1}{U}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchanted creature has flying.{EOL}When Launch is put into a graveyard from the battlefield, return Launch to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddStaticAbility(Static.Flying));
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment(filter: x => !x.Has().Flying));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Launch is put into a graveyard from the battlefield, return Launch to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}