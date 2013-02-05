namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class Bravado : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Bravado")
        .ManaCost("{1}{R}")
        .Type("Enchantment Aura")
        .Text("{Enchant creature}{EOL}Enchanted creature gets +1/+1 for each other creature you control.")
        .FlavorText("We drive the dragons from our home. Why should we fear you?{EOL}—Fire Eye, viashino bey")
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