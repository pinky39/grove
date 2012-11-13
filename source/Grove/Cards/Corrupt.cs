namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Corrupt : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Corrupt")
        .ManaCost("{5}{B}")
        .Type("Sorcery")
        .Text(
          "Corrupt deals damage equal to the number of Swamps you control to target creature or player. You gain life equal to the damage dealt this way.")
        .FlavorText("Yawgmoth brushed Urza's mind, and Urza's world convulsed.")
        .Effect<DealDamageToTargets>(p =>
          {
            p.Amount = p.Controller.Battlefield.Count(x => x.Is("swamp"));                            
            p.GainLife = true;
          })
        .Timing(Timings.MainPhases())
        .Targets(
          selectorAi: TargetSelectorAi.DealDamageSingleSelector(p => p.Controller.Battlefield.Count(x => x.Is("swamp"))),
          effectValidator: Validator(Validators.CreatureOrPlayer()));
    }
  }
}