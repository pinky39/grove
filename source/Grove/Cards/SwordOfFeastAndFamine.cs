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
            C.Effect<CompoundEffect>(p => p.Effect.ChildEffects(
              p.Builder.Effect<OpponentDiscardsCards>(e1 => e1.SelectedCount = 1),
              p.Builder.Effect<UntapAllLands>()
              ))),
          C.ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = 2.AsColorlessMana()),
            C.Effect<AttachEquipment>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              p.Builder.Modifier<AddProtectionFromColors>((m, _) => m.Colors = ManaColors.Black | ManaColors.Green))),
            effectValidator: C.Validator(Validators.Equipment()),
            aiTargetFilter: AiTargetSelectors.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(),
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector));
    }
  }
}