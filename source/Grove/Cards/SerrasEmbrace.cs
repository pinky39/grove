namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class SerrasEmbrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Serra's Embrace")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}Enchanted creature gets +2/+2 and has flying and vigilance.")
        .FlavorText(
          "'Lifted beyond herself, for that battle Brindri was an angel of light and fury.'{EOL}—Song of All, canto 524")
        .Effect<EnchantCreature>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 2;
              m.Toughness = 2;
            }),
          p.Builder.Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Vigilance),
          p.Builder.Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)))
        .Timing(Timings.FirstMain())
        .Targets(
          filter: TargetFilters.CombatEnchantment(),
          effect: C.Selector(Selectors.EnchantedCreature()));
    }
  }
}