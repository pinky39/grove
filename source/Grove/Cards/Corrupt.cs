namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Corrupt : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Corrupt")
        .ManaCost("{5}{B}")
        .Type("Sorcery")
        .Text(
          "Corrupt deals damage equal to the number of Swamps you control to target creature or player. You gain life equal to the damage dealt this way.")
        .FlavorText("Yawgmoth brushed Urza's mind, and Urza's world convulsed.")
        .Effect<DealDamageToTarget>((e, _) =>
          {
            e.SetAmount(self => self.Controller.Battlefield.Count(x => x.Is("swamp")));
            e.GainLife = true;
          })
        .Timing(Timings.MainPhases())
        .Targets(
          filter: TargetFilters.DealDamage(p => p.Controller.Battlefield.Count(x => x.Is("swamp"))),
          effect: C.Selector(Selectors.CreatureOrPlayer()));
    }
  }
}