namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class InfernoFist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Inferno Fist")
        .ManaCost("{1}{R}")
        .Type("Enchantment — Aura")
        .Text("Enchant creature you control{EOL}Enchanted creature gets +2/+0.{EOL}{R}, Sacrifice Inferno Fist: Inferno Fist deals 2 damage to target creature or player.")
        .Cast(p =>
        {
          p.Effect = () => new Attach(
            () => new AddPowerAndToughness(2, 0)).SetTags(EffectTag.IncreasePower);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature(controlledBy: ControlledBy.SpellOwner).On.Battlefield());

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectCombatEnchantment());
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{R}, Sacrifice Inferno Fist: Inferno Fist deals 2 damage to target creature or player.";

          p.Cost = new AggregateCost(
            new PayMana(Mana.Red),
            new Sacrifice());

          p.Effect = () => new DealDamageToTargets(2);
          p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
          p.TargetingRule(new EffectDealDamage(2));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
