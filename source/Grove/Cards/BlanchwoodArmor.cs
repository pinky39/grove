namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class BlanchwoodArmor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Blanchwood Armor")
        .ManaCost("{2}{G}")
        .Type("Enchantment - Aura")
        .Text("Enchanted creature gets +1/+1 for each Forest you control.")
        .FlavorText("Before armor, there was bark. Before blades, there were thorns.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new Add11ForEachForest());
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new FirstMain());
            p.TargetingRule(new CombatEnchantment());
          });
    }
  }
}