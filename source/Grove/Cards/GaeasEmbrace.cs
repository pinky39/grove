namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Targeting;

  public class GaeasEmbrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Gaea's Embrace")
        .ManaCost("{2}{G}{G}")
        .Type("Enchantment - Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature gets +3/+3 and has trample.{EOL}{G}: Regenerate enchanted creature.")
        .FlavorText("The forest rose to the battle, not to save the people but to save itself.")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<Core.Effects.Attach>(e => e.Modifiers(
              Modifier<AddActivatedAbility>(m => m.Ability =
                ActivatedAbility(
                  "{G}: Regenerate enchanted creature.",
                  Cost<PayMana>(cost => cost.Amount = ManaAmount.Green),
                  Effect<Regenerate>(),
                  timing: Timings.Regenerate())
                ),
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 3;
                  m.Toughness = 3;
                }),
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Trample)
              ));
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.CombatEnchantment();
          });
    }
  }
}