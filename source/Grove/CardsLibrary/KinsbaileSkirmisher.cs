namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class KinsbaileSkirmisher : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kinsbaile Skirmisher")
        .ManaCost("{1}{W}")
        .Type("Creature - Kithkin Soldier")
        .Text("When Kinsbaile Skirmisher enters the battlefield, target creature gets +1/+1 until end of turn.")
        .FlavorText("\"If a boggart even dares breathe near one of my kin, I'll know. And I'll not be happy.\"")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "When Kinsbaile Skirmisher enters the battlefield, target creature gets +1/+1 until end of turn.";

            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield));

            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(1, 1) {UntilEot = true})
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}