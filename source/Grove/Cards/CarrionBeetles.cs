namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Player;

  public class CarrionBeetles : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Carrion Beetles")
        .ManaCost("{B}")
        .Type("Creature Insect")
        .Text("{2}{B},{T}: Exile up to three target cards from a single graveyard.")
        .FlavorText("It's all fun and games until someone loses an eye.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{B},{T}: Exile up to three target cards from a single graveyard.";
            p.Cost = new AggregateCost(
              new PayMana("{2}{B}".Parse(), ManaUsage.Abilities),
              new Tap());
            p.Effect = () => new ExileTargets();
            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card().In.Graveyard();
                trg.MinCount = 0;
                trg.MaxCount = 3;
              });
            p.TimingRule(new EndOfTurn());
            p.TargetingRule(new OrderByRank(c => -c.Score, ControlledBy.Opponent));
          });
    }
  }
}