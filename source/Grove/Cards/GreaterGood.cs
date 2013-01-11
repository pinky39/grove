namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class GreaterGood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Greater Good")
        .ManaCost("{2}{G}{G}")
        .Type("Enchantment")
        .Text("Sacrifice a creature: Draw cards equal to the sacrificed creature's power, then discard three cards.")
        .FlavorText("'We have more sprouts than they have hands.'{EOL}—Gamelen, Citanul elder")
        .Cast(p => p.Timing = All(Timings.OnlyOneOfKind(), Timings.FirstMain()))                
        .Abilities(
          ActivatedAbility(
            "Sacrifice a creature: Draw cards equal to the sacrificed creature's power, then discard three cards.",
            Cost<Sacrifice>(),
            Effect<DrawCards>(e =>
              {
                e.DrawCount = e.CostTarget().Card().Power.GetValueOrDefault();
                e.DiscardCount = 3;
              }),
            costTarget:
              Target(
                Validators.Card(ControlledBy.SpellOwner, x => x.Is().Creature),
                Zones.Battlefield(),
                text: "Select a creature to sacrifice.", mustBeTargetable: false),
            targetingAi: TargetingAi.CostSacrificeDrawCards(x => x.Power > 3))
        );
    }
  }
}