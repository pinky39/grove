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
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Category = EffectCategories.ToughnessIncrease;
            p.Effect = Effect<Core.Cards.Effects.Attach>(e => e.Modifiers(Modifier<Add11ForEachOtherCreature>()));
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Creature), Zones.Battlefield()));
            p.TargetSelectorAi = TargetSelectorAi.CombatEnchantment();
          });
    }
  }
}