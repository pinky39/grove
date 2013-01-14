namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Targeting;

  public class Blaze : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Blaze")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("Blaze deals X damage to target creature or player.")
        .FlavorText("Fire never dies alone.")
        .Cast(p =>
          {
            p.Timing = Timings.MainPhases();
            p.Effect = Effect<Core.Effects.DealDamageToTargets>(e => e.Amount = Value.PlusX);
            p.XCalculator = ChooseXAi.TargetLifepointsLeft(ManaUsage.Spells);
            p.EffectTargets = L(Target(Validators.CreatureOrPlayer(), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.DealDamageSingleSelector();
          });
    }
  }
}