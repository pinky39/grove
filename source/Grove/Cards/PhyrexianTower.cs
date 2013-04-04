namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class PhyrexianTower : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Effect = () => new AddManaToPool("{B}{B}".ParseMana());
            p.TargetSelector.AddCost(trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield());
                        
            p.TimingRule(new ControllerNeedsAdditionalMana(2));
            p.TargetingRule(new OrderByRank(c => c.Score));

            p.UsesStack = false;
          });
    }
  }
}