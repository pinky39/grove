namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class ShivsEmbrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Shiv's Embrace")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment - Aura")
        .Text("Enchant creature{EOL}Enchanted creature gets +2/+2 and has flying.{EOL}{R}: Enchanted creature gets +1/+0 until end of turn.")
        .FlavorText("Wear the foe's form to best it in battle. So sayeth the bey.")
        .Timing(Timings.FirstMain())
        .Effect<EnchantCreature>(p => p.Effect.Modifiers(
          p.Builder.Modifier<AddActivatedAbility>((m, c) => m.Ability =
            c.ActivatedAbility(
              "{R}: Enchanted creature gets +1/+0 until end of turn.",
              c.Cost<TapOwnerPayMana>(cost => cost.Amount = ManaAmount.Red),
              c.Effect<ApplyModifiersToSelf>(p1 => p1.Effect.Modifiers(
                p1.Builder.Modifier<AddPowerAndToughness>(m1 => m1.Power = 1, untilEndOfTurn: true))),
              timing: Timings.IncreaseOwnersPowerAndThougness(1, 0))
            ),
          p.Builder.Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 2;
              m.Toughness = 2;
            }),
          p.Builder.Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)
          ))
        .Targets(
          aiTargetSelector: AiTargetSelectors.CombatEnchantment(),
          effectValidator: C.Validator(Validators.EnchantedCreature()));
    }
  }
}