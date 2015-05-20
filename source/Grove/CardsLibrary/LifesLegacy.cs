namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class LifesLegacy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Life's Legacy")
        .ManaCost("{1}{G}")
        .Type("Sorcery")
        .Text(
          "As an additional cost to cast Life's Legacy, sacrifice a creature.{EOL}Draw cards equal to the sacrificed creature's power.")
        .FlavorText("At the instant of death, the mystery of life.")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana("{1}{G}".Parse()),
              new Sacrifice());

            p.TargetSelector.AddCost(
              trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield(),
              trg => { trg.Message = "Select a creature to sacrifice."; });

            p.Effect = () => new DrawCards(
              count: P(e => e.Target.Card().Power.GetValueOrDefault()));

            p.TargetingRule(new EffectOrCostRankBy(c => c.Score));
          });
    }
  }
}