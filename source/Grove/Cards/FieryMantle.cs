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
  using Core.Zones;

  public class FieryMantle : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fiery Mantle")
        .ManaCost("{1}{R}")
        .Type("Enchantment - Aura")
        .Text(
          "Enchant creature{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.{EOL}When Fiery Mantle is put into a graveyard from the battlefield, return Fiery Mantle to its owner's hand.")
        .Timing(Timings.FirstMain())
        .Effect<Attach>(e => e.Modifiers(
          Modifier<AddActivatedAbility>(m => m.Ability =
            ActivatedAbility(
              "{R}: Enchanted creature gets +1/+0 until end of turn.",
              Cost<PayMana>(cost => cost.Amount = ManaAmount.Red),
              Effect<ApplyModifiersToSelf>(p1 => p1.Effect.Modifiers(
                Modifier<AddPowerAndToughness>(m1 => m1.Power = 1, untilEndOfTurn: true))),
              timing: Timings.IncreaseOwnersPowerAndThougness(1, 0))
            )))
        .Targets(
          TargetSelectorAi.CombatEnchantment(),
          Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()))
        .Abilities(
          TriggeredAbility(
            "When Fiery Mantle is put into a graveyard from the battlefield, return Fiery Mantle to its owner's hand.",
            Trigger<OnZoneChange>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<PutToHand>(e => e.ReturnOwner = true)));
    }
  }
}