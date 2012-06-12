namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class VolcanicHammer : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Volcanic Hammer")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Volcanic Hammer deals 3 damage to target creature or player.")
        .FlavorText("Fire finds its form in the heat of the forge.")
        .Category(EffectCategories.DamageDealing)
        .Effect<DealDamageToTarget>((e, _) => e.Amount = 3)
        .Target(C.Selector(
          validator: target => target.IsPlayer() || target.Is().Creature,
          scorer: Core.Ai.TargetScores.OpponentStuffScoresMore(spellsDamage: 3)));
    }
  }
}