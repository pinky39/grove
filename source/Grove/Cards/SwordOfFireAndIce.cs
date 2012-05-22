namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;

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
        .Timing(Timings.Steps(Step.FirstMain))
        .Abilities(
          C.TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target creature or player and you draw a card.",
            C.Trigger<DealDamageToPlayer>((t, _) =>
            {
              t.CombatOnly = true;
              t.ToAny();
            }),
            C.Effect<CompoundEffect>((e, c) => e.ChildEffects(
              c.Effect<DealDamageToTarget>((e1, _) => e1.Amount = 2),
              c.Effect<DrawCards>((e2, _) => e2.DrawCount = 1))),
            C.Selector(
              validator: target => target.IsPlayer() || target.Is().Creature,
              scorer: TargetScores.OpponentStuffScoresMore(spellsDamage: 2)),
            category: EffectCategories.DamageDealing),
          C.ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = 2),
            C.Effect<AttachEquipment>((e, c) => e.Modifiers(
              c.Modifier<AddPowerAndToughness>((m, _) =>
              {
                m.Power = 2;
                m.Toughness = 2;
              }),
              C.Modifier<AddProtectionFromColors>((m, _) => m.Colors = ManaColors.Red | ManaColors.Blue)
              )),
            selector: C.Selector(Validator.Equipment()),
            activateAsSorcery: true
            ));
    }
  }  
}