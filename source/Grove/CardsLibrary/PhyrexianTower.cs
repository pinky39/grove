namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class PhyrexianTower : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Tower")
        .Type("Legendary Land")
        .Text("{T}: Add {1} to your mana pool.{EOL}{T}, Sacrifice a creature: Add {B}{B} to your mana pool.")
        .FlavorText("Living metal encasing dying flesh.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {1} to your mana pool.";
            p.ManaAmount(1.Colorless());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{T}, Sacrifice a creature: Add {B}{B} to your mana pool.";
            p.Cost = new AggregateCost(new Tap(), new Sacrifice());
            p.Effect = () => new AddManaToPool("{B}{B}".Parse());
            p.TargetSelector.AddCost(trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield());

            p.TimingRule(new WhenYouNeedAdditionalMana(2));
            p.TargetingRule(new EffectRankBy(c => c.Score) {TargetLimit = 1});

            p.UsesStack = false;
          });
    }
  }
}