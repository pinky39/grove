namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;

  public class SwordOfFeastAndFamine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sword of Feast and Famine")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from black and from green.{EOL}Whenever equipped creature deals combat damage to a player, that player discards a card and you untap all lands you control.{EOL}{Equip} {2}")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, that player discards a card and you untap all lands you control.",
            C.Trigger<DealDamageToCreatureOrPlayer>((t, _) =>
            {
              t.CombatOnly = true;
              t.UseAttachedToAsTriggerSource = true;
              t.ToAnyPlayer();
            }),
            C.Effect<CompoundEffect>((e, c) => e.ChildEffects(
              c.Effect<OpponentDiscardsCards>((e1, _) => e1.SelectedCount = 1),
              c.Effect<UntapAllLands>()
              ))),
          C.ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = 2.AsColorlessMana()),
            C.Effect<AttachEquipment>((e, c) => e.Modifiers(
              c.Modifier<AddPowerAndToughness>((m, _) =>
              {
                m.Power = 2;
                m.Toughness = 2;
              }),
              c.Modifier<AddProtectionFromColors>((m, _) => m.Colors = ManaColors.Black | ManaColors.Green))),
            effectSelector: C.Selector(Selectors.Equipment()), 
            targetFilter:TargetFilters.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(), 
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector));
    }
  }
}