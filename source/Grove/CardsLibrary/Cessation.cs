namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

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
              () => new AddSimpleAbility(Static.CannotAttack)).SetTags(EffectTag.CombatDisabler);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectCannotBlockAttack(attackOnly: true));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Cessation is put into a graveyard from the battlefield, return Cessation to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}