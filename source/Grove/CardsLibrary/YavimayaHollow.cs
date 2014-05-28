namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class YavimayaHollow : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yavimaya Hollow")
        .Type("Legendary Land")
        .Text("{T}: Add {1} to your mana pool.{EOL}{G},{T}: Regenerate target creature.")
        .FlavorText("For all its traps and defenses, Yavimaya has its havens, too.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {1} to your mana pool.";
            p.ManaAmount(1.Colorless());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{G},{T}: Regenerate target creature.";
            p.Cost = new AggregateCost(
              new PayMana("{G}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new RegenerateTarget();
            p.TargetSelector.AddEffect(trg =>
              trg.Is.Card(c => c.Is().Creature));

            p.TimingRule(new RegenerateTargetTimingRule());
            p.TargetingRule(new EffectGiveRegenerate());
          });
    }
  }
}