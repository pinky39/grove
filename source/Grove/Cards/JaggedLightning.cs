namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class JaggedLightning : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Jagged Lightning")
        .ManaCost("{3}{R}{R}")
        .Type("Sorcery")
        .Text("Jagged Lightning deals 3 damage to each of two target creatures.")
        .FlavorText(
          "The pungent smell of roasting flesh made both mages realize they'd rather break for dinner than fight.")
        .Cast(p =>
          {
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = 3);
            p.EffectTargets = L(Target(Validators.Card(card => card.Is().Creature),
              Zones.Battlefield(), minCount: 2, maxCount: 2));
            p.TargetingAi = TargetingAi.DealDamageSingleSelector(3);
          });
    }
  }
}