namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class Cessation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cessation")
        .ManaCost("{2}{W}")
        .Type("Enchantment Aura")
        .Text(
          "Enchanted creature can't attack.{EOL}When Cessation is put into a graveyard from the battlefield, return Cessation to its owner's hand.")
        .FlavorText("The face of Light will shine upon them, and they will know war no more.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.CannotAttack)).Tags(EffectTag.CombatDisabler);              
              
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectCannotBlockAttack(attackOnly: true));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Cessation is put into a graveyard from the battlefield, return Cessation to its owner's hand.";
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}