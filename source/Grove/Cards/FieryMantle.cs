namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Targeting;
  using Core.Triggers;
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
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Core.Effects.Attach>(e => e.Modifiers(
              Modifier<AddActivatedAbility>(m => m.Ability =
                ActivatedAbility(
                  "{R}: Enchanted creature gets +1/+0 until end of turn.",
                  Cost<PayMana>(cost => cost.Amount = ManaAmount.Red),
                  Effect<Core.Effects.ApplyModifiersToSelf>(p1 => p1.Effect.Modifiers(
                    Modifier<AddPowerAndToughness>(m1 => m1.Power = 1, untilEndOfTurn: true))),
                  timing: Timings.IncreaseOwnersPowerAndThougness(1, 0))
                )));
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.CombatEnchantment();
          })
        .Abilities(
          TriggeredAbility(
            "When Fiery Mantle is put into a graveyard from the battlefield, return Fiery Mantle to its owner's hand.",
            Trigger<OnZoneChanged>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<Core.Effects.ReturnToHand>(e => e.ReturnOwner = true)));
    }
  }
}