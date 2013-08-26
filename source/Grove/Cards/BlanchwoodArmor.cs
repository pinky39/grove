namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class BlanchwoodArmor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blanchwood Armor")
        .ManaCost("{2}{G}")
        .Type("Enchantment - Aura")
        .Text("Enchanted creature gets +1/+1 for each Forest you control.")
        .FlavorText("Before armor, there was bark. Before blades, there were thorns.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              new ModifyPowerToughnessForEachPermanent(
                power: 1,
                toughness: 1,
                filter: (c, _) => c.Is("forest"),
                modifier: () => new IntegerIncrement()))
              {
                Category = EffectCategories.ToughnessIncrease
              };

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatEnchantment());
          });
    }
  }
}