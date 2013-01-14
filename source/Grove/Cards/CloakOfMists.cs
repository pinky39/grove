namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;
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
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Core.Effects.Attach>(e => e.Modifiers(Modifier<AddStaticAbility>(
              m => m.StaticAbility = Static.Unblockable)));
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.CombatEnchantment();
          });
    }
  }
}