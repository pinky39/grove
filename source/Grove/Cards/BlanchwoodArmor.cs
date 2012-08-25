namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class BlanchwoodArmor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Blanchwood Armor")
        .ManaCost("{2}{G}")
        .Type("Enchantment - Aura")
        .Text("{Enchant creature}{EOL}Enchanted creature gets +1/+1 for each Forest you control.")
        .FlavorText("'Before armor, there was bark. Before blades, there were thorns.'{EOL}—Molimo, maro-sorcerer")
        .Effect<Attach>(p => p.Effect.Modifiers(p.Builder.Modifier<Add11ForEachForest>()))
        .Category(EffectCategories.ToughnessIncrease)
        .Timing(Timings.FirstMain())
        .Targets(
          selectorAi: TargetSelectorAi.CombatEnchantment(),
          effectValidator: C.Validator(Validators.EnchantedCreature()));
    }
  }
}