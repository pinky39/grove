namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Modifiers;
  using Core.Targeting;

  public class HermeticStudy : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hermetic Study")
        .ManaCost("{1}{U}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature has '{T}: This creature deals 1 damage to target creature or player.'")
        .FlavorText("'Books can be replaced; a prize student cannot. Be patient.'{EOL}—Urza, to Barrin")
        .Cast(p =>
          {
            p.Timing = Timings.SecondMain();
            p.Effect = Effect<Core.Effects.Attach>(e => e.Modifiers(
              Modifier<AddActivatedAbility>(m => m.Ability =
                ActivatedAbility(
                  "{T}: This creature deals 1 damage to target creature or player.",
                  Cost<Tap>(),
                  Effect<Core.Effects.DealDamageToTargets>(e1 => e1.Amount = 1),
                  Target(Validators.CreatureOrPlayer(), Zones.Battlefield()),
                  targetingAi: TargetingAi.DealDamageSingleSelector(1),
                  timing: p.Timing = Timings.InstantRemovalTarget())
                )));
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.OrderByScore(descending: false);
          });
    }
  }
}