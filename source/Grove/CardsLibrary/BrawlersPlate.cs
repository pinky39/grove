namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class BrawlersPlate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Brawler's Plate")
        .ManaCost("{3}")
        .Type("Artifact — Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has trample.{I}(If it would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}{EOL}Equip{4} {I}({4}: Attach to target creature you control. Equip only as a sorcery.){/I}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{4}: Attach to target creature you control. Equip only as a sorcery.";

            p.Cost = new PayMana(4.Colorless(), ManaUsage.Abilities);

            p.Effect = () => new Attach(
              () => new AddStaticAbility(Static.Trample),
              () => new AddPowerAndToughness(2, 2))
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());

            p.TargetingRule(new EffectCombatEquipment());
            p.TimingRule(new OnFirstDetachedOnSecondAttached());

            p.ActivateAsSorcery = true;
            p.IsEquip = true;
          });
    }
  }
}