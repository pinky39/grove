namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class FlameSlash : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Flame Slash")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("Flame Slash deals 4 damage to target creature.")
        .FlavorText(
          "After millennia asleep, the Eldrazi had forgotten about Zendikar's fiery temper and dislike of strangers.")
        .Effect<DealDamageToTargets>(e => e.Amount = 4)
        .Timing(Timings.InstantRemovalTarget())
        .Targets(
          TargetSelectorAi.DealDamageSingleSelector(4),
          Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
    }
  }
}