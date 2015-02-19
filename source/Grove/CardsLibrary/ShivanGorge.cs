namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class ShivanGorge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shivan Gorge")
        .Type("Legendary Land")
        .Text("{T}: Add {1} to your mana pool.{EOL}{2}{R},{T}: Shivan Gorge deals 1 damage to each opponent.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {1} to your mana pool.";
            p.ManaAmount(1.Colorless());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{R},{T}: Shivan Gorge deals 1 damage to each opponent.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{R}".Parse()),
              new Tap());

            p.Effect = () => new DealDamageToPlayer(1, P(e => e.Controller.Opponent));
            p.TimingRule(new OnStep(Step.EndOfTurn));
          });
    }
  }
}