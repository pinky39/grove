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
  using Core.Zones;

  public class FieryMantle : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Fiery Mantle")
        .ManaCost("{1}{R}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.{EOL}When Fiery Mantle is put into a graveyard from the battlefield, return Fiery Mantle to its owner's hand.")
        .Timing(Timings.FirstMain())
        .Effect<Attach>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddActivatedAbility>((m, c) => m.Ability =
            c.ActivatedAbility(
              "{R}: Enchanted creature gets +1/+0 until end of turn.",
              c.Cost<TapOwnerPayMana>(cost => cost.Amount = ManaAmount.Red),
              c.Effect<ApplyModifiersToSelf>(p1 => p1.Effect.Modifiers(
                p1.Builder.Modifier<AddPowerAndToughness>(m1 => m1.Power = 1, untilEndOfTurn: true))),
              timing: Timings.IncreaseOwnersPowerAndThougness(1, 0))
            )))
        .Targets(
          selectorAi: TargetSelectorAi.CombatEnchantment(),
          effectValidator: C.Validator(Validators.EnchantedCreature()))
        .Abilities(
          C.TriggeredAbility(
            "When Fiery Mantle is put into a graveyard from the battlefield, return Fiery Mantle to its owner's hand.",
            C.Trigger<OnZoneChange>((t, _) =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            C.Effect<ReturnToHand>(e => e.ReturnOwner = true)));
    }
  }
}