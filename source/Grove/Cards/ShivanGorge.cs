namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.States;

  public class ShivanGorge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
              new PayMana("{2}{R}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new DealDamageToPlayer(1, P(e => e.Controller.Opponent));
            p.TimingRule(new Steps(steps: Step.EndOfTurn, activeTurn: true, passiveTurn: true));
          });
    }
  }
}