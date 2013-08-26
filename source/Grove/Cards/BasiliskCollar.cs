namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class BasiliskCollar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Basilisk Collar")
        .ManaCost("{1}")
        .Type("Artifact - Equipment")
        .Text("Equipped creature has deathtouch and lifelink.{EOL}{Equip} {2}")
        .FlavorText(
          "During their endless travels, the mages of the Goma Fada caravan have learned ways to harness both life and death.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Attach to target creature you control. Equip only as a sorcery.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.Deathtouch),
              () => new AddStaticAbility(Static.Lifelink));
            p.TargetSelector
              .AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());
            p.TargetingRule(new EffectCombatEquipment());
            p.TimingRule(new OnFirstDetachedOnSecondAttached());
            p.ActivateAsSorcery = true;
          });
    }
  }
}