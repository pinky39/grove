namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Targeting;
  using Core.Triggers;

  public class SwordOfFireAndIce : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sword of Fire and Ice")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from red and from blue.{EOL}Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.{EOL}{Equip} {2}")
        .Cast(p => p.Timing = Timings.FirstMain())
        .Abilities(
          TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.",
            Trigger<OnDamageDealt>(t =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToPlayer();
              }),
            Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<DealDamageToTargets>(e1 => e1.Amount = 2),
              Effect<DrawCards>(e1 => e1.Count = 1))),
            Target(
              Validators.CreatureOrPlayer(), Zones.Battlefield()),
            selectorAi: TargetingAi.DealDamageSingleSelector(2)),
          ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            Cost<PayMana>(cost => cost.Amount = 2.Colorless()),
            Effect<Attach>(p => p.Effect.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              Modifier<AddProtectionFromColors>(m => m.Colors = ManaColors.Red | ManaColors.Blue)
              )),
            effectTarget: Target(Validators.ValidEquipmentTarget(), Zones.Battlefield()),
            targetingAi: TargetingAi.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(),
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector
            ));
    }
  }
}