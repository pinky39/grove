namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Bravado : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Bravado")
        .ManaCost("{1}{R}")
        .Type("Enchantment Aura")
        .Text("{Enchant creature}{EOL}Enchanted creature gets +1/+1 for each other creature you control.")
        .FlavorText("We drive the dragons from our home. Why should we fear you?{EOL}—Fire Eye, viashino bey")
        .Effect<Core.Cards.Effects.Attach>(p => p.Effect.Modifiers(Modifier<Add11ForEachOtherCreature>()))
        .Category(EffectCategories.ToughnessIncrease)
        .Timing(Timings.FirstMain())
        .Targets(
          selectorAi: TargetSelectorAi.CombatEnchantment(),
          effectValidator: Validator(Validators.EnchantedCreature()));        
    }
  }
}