namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

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
        .Timing(Timings.FirstMain())
        .Abilities(
          TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.",
            Trigger<DealDamageToCreatureOrPlayer>(t =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToPlayer();
              }),
            Effect<Core.Cards.Effects.CompoundEffect>(e => e.ChildEffects(
              Effect<Core.Cards.Effects.DealDamageToTargets>(e1 => e1.Amount = 2),
              Effect<Core.Cards.Effects.DrawCards>(e1 => e1.DrawCount = 1))),
            Validator(
              Validators.CreatureOrPlayer()),
            selectorAi: TargetSelectorAi.DealDamageSingleSelector(2)),
          ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            Cost<TapOwnerPayMana>(cost => cost.Amount = 2.AsColorlessMana()),
            Effect<Core.Cards.Effects.Attach>(p => p.Effect.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              Modifier<AddProtectionFromColors>(m => m.Colors = ManaColors.Red | ManaColors.Blue)
              )),
            effectValidator: Validator(Validators.Equipment()),
            selectorAi: TargetSelectorAi.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(),
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector
            ));
    }
  }
}