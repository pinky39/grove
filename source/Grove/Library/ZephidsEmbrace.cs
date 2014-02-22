namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class ZephidsEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Zephid's Embrace")
        .ManaCost("{2}{U}{U}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchanted creature gets +2/+2 and has flying and shroud.")
        .FlavorText("Spells will shun you, as will everyone else.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddStaticAbility(Static.Flying),
              () => new AddStaticAbility(Static.Shroud)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}