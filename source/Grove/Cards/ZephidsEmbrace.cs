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
              () => new AddStaticAbility(Static.Shroud)) {Kinds = EffectTag.IncreaseToughness};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}