namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class ShivsEmbrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Shiv's Embrace")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment - Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature gets +2/+2 and has flying.{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.")
        .FlavorText("Wear the foe's form to best it in battle. So sayeth the bey.")
        .Timing(Timings.FirstMain())
        .Effect<Attach>(e => e.Modifiers(
          Modifier<AddActivatedAbility>(m => m.Ability =
            ActivatedAbility(
              "{R}: Enchanted creature gets +1/+0 until end of turn.",
              Cost<PayMana>(cost => cost.Amount = ManaAmount.Red),
              Effect<ApplyModifiersToSelf>(e1 => e1.Modifiers(
                Modifier<AddPowerAndToughness>(m1 => m1.Power = 1, untilEndOfTurn: true))),
              timing: Timings.IncreaseOwnersPowerAndThougness(1, 0))
            ),
          Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 2;
              m.Toughness = 2;
            }),
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)
          ))
        .Targets(
          TargetSelectorAi.CombatEnchantment(), 
          TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield()));
    }
  }
}