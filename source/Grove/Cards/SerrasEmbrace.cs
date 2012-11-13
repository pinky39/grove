namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class SerrasEmbrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Serra's Embrace")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}Enchanted creature gets +2/+2 and has flying and vigilance.")
        .FlavorText(
          "'Lifted beyond herself, for that battle Brindri was an angel of light and fury.'{EOL}—Song of All, canto 524")
        .Effect<Attach>(e => e.Modifiers(
          Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 2;
              m.Toughness = 2;
            }),
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Vigilance),
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)))
        .Timing(Timings.FirstMain())
        .Targets(
          selectorAi: TargetSelectorAi.CombatEnchantment(),
          effectValidator: Validator(Validators.EnchantedCreature()));
    }
  }
}