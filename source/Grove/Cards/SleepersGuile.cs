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

  public class SleepersGuile : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sleeper's Guile")
        .ManaCost("{2}{B}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature has fear. (It can't be blocked except by artifact creatures and/or black creatures.){EOL}When Sleeper's Guile is put into a graveyard from the battlefield, return Sleeper's Guile to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.Fear));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment(c => !c.Has().AnyEvadingAbility));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Sleeper's Guile is put into a graveyard from the battlefield, return Sleeper's Guile to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}