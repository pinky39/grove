namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class SwordOfFeastAndFamine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sword of Feast and Famine")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped creature gets +2/+2 and has protection from black and from green.{EOL}Whenever equipped creature deals combat damage to a player, that player discards a card and you untap all lands you control.{EOL}{Equip} {2}")
        .Timing(Timings.FirstMain())
        .Abilities(
          TriggeredAbility(
            "Whenever equipped creature deals combat damage to a player, that player discards a card and you untap all lands you control.",
            Trigger<DealDamageToCreatureOrPlayer>(t =>
              {
                t.CombatOnly = true;
                t.UseAttachedToAsTriggerSource = true;
                t.ToPlayer();
              }),
            Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<OpponentDiscardsCards>(e1 => e1.SelectedCount = 1),
              Effect<UntapAllLands>()
              ))),
          ActivatedAbility(
            "{2}: Attach to target creature you control. Equip only as a sorcery.",
            Cost<TapOwnerPayMana>(cost => cost.Amount = 2.AsColorlessMana()),
            Effect<Attach>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              Modifier<AddProtectionFromColors>(m => m.Colors = ManaColors.Black | ManaColors.Green))),
            effectValidator: TargetValidator(TargetIs.ValidEquipmentTarget(), ZoneIs.Battlefield()),
            targetSelectorAi: TargetSelectorAi.CombatEquipment(),
            timing: Timings.AttachCombatEquipment(),
            activateAsSorcery: true,
            category: EffectCategories.ToughnessIncrease | EffectCategories.Protector));
    }
  }
}