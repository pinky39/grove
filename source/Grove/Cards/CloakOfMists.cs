namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class CloakOfMists : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Cloak of Mists")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text("{Enchant creature}{EOL}Enchanted creature is unblockable.")
        .FlavorText(
          "'All we could lose, we did. All we could keep, we do. And both are shrouded by mists.'{EOL}—Barrin, master wizard")
        .Timing(Timings.FirstMain())
        .Effect<Attach>(e => e.Modifiers(
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Unblockable)))
        .Targets(
          selectorAi: TargetSelectorAi.CombatEnchantment(),
          effectValidator: Validator(Validators.EnchantedCreature()));
    }
  }
}