namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Targeting;

  public class VolcanicHammer : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Volcanic Hammer")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Volcanic Hammer deals 3 damage to target creature or player.")
        .FlavorText("Fire finds its form in the heat of the forge.")
        .Cast(p =>
          {
            p.Effect = Effect<Core.Effects.DealDamageToTargets>(e => e.Amount = 3);
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.DealDamageSingleSelector(3);
          });
    }
  }
}