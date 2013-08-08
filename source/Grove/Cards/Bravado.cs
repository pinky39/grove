namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Bravado : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bravado")
        .ManaCost("{1}{R}")
        .Type("Enchantment Aura")
        .Text("Enchanted creature gets +1/+1 for each other creature you control.")
        .FlavorText("We drive the dragons from our home. Why should we fear you?")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new Add11ForEachOtherCreature())
              {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}