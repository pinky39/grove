namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class SwordOfFireAndIce : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sword of Fire and Ice")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from red and from blue.{EOL}Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.{EOL}{Equip} {2}")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.",
            C.Trigger<DealDamageToCreatureOrPlayer>((t, _) =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToAnyPlayer();
              }),
            C.Effect<CompoundEffect>(p => p.Effect.ChildEffects(
              p.Builder.Effect<DealDamageToTarget>(e1 => e1.Amount = 2),
              p.Builder.Effect<DrawCards>(e1 => e1.DrawCount = 1))),
            C.Selector(
              Selectors.CreatureOrPlayer()),
            targetFilter: TargetFilters.DealDamage(2)),
          C.ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = 2.AsColorlessMana()),
            C.Effect<AttachEquipment>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              p.Builder.Modifier<AddProtectionFromColors>((m, _) => m.Colors = ManaColors.Red | ManaColors.Blue)
              )),
            effectSelector: C.Selector(Selectors.Equipment()),
            targetFilter: TargetFilters.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(),
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector
            ));
    }
  }
}