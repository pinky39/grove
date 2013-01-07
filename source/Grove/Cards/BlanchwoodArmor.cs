namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class BlanchwoodArmor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Blanchwood Armor")
        .ManaCost("{2}{G}")
        .Type("Enchantment - Aura")
        .Text("{Enchant creature}{EOL}Enchanted creature gets +1/+1 for each Forest you control.")
        .FlavorText("'Before armor, there was bark. Before blades, there were thorns.'{EOL}—Molimo, maro-sorcerer")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Category = EffectCategories.ToughnessIncrease;
            p.Effect = Effect<Attach>(e => e.Modifiers(Modifier<Add11ForEachForest>()));
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Creature), Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.CombatEnchantment();
          });
    }
  }
}