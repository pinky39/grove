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
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Core.Effects.Attach>(e => e.Modifiers(
              Modifier<AddActivatedAbility>(m => m.Ability =
                ActivatedAbility(
                  "{R}: Enchanted creature gets +1/+0 until end of turn.",
                  Cost<PayMana>(cost => cost.Amount = ManaAmount.Red),
                  Effect<Core.Effects.ApplyModifiersToSelf>(e1 => e1.Modifiers(
                    Modifier<AddPowerAndToughness>(m1 => m1.Power = 1, untilEndOfTurn: true))),
                  timing: Timings.IncreaseOwnersPowerAndThougness(1, 0))
                ),
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }),
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying)
              ));
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.CombatEnchantment();
          });                
    }
  }
}