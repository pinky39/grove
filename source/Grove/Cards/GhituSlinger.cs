namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class GhituSlinger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ghitu Slinger")
        .ManaCost("{2}{R}")
        .Type("Creature Human Nomad")
        .Text(
          "{Echo} {2}{R}{EOL}When Ghitu Slinger enters the battlefield, it deals 2 damage to target creature or player.")
        .FlavorText("If you survive a slinger attack, the slinger wished you to live.")
        .Power(2)
        .Toughness(2)
        .Echo("{2}{R}")
        .TriggeredAbility(p =>
          {
            p.Text = "When Ghitu Slinger enters the battlefield, it deals 2 damage to target creature or player.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DealDamageToTargets(2);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(2));
          });
    }
  }
}