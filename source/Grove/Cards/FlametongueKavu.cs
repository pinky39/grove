namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class FlametongueKavu : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Flametongue Kavu")
        .ManaCost("{3}{R}")
        .Type("Creature Kavu")
        .Text("When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.")
        .FlavorText("'For dim-witted, thick-skulled genetic mutants, they have pretty good aim.{EOL}—Sisay'")
        .Power(4)
        .Toughness(2)
        .Abilities(
          C.TriggeredAbility(
            "When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<DealDamageToTarget>((e, _) => e.Amount = 4),
            C.Selector(
              validator: target => target.Is().Creature,
              scorer: Core.Ai.TargetScores.OpponentStuffScoresMore(4)),
            category: EffectCategories.DamageDealing)
        );
    }

   
  }
}