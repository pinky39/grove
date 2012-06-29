namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class FlameSlash : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Flame Slash")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("Flame Slash deals 4 damage to target creature.")
        .FlavorText(
          "After millennia asleep, the Eldrazi had forgotten about Zendikar's fiery temper and dislike of strangers.")
        .Effect<DealDamageToTarget>((e, _) => e.Amount = 4)
        .Targets(C.Selector(
          validator: target => target.Is().Creature,
          scorer: TargetScores.OpponentStuffScoresMore(spellsDamage: 4)));
    }
  }
}