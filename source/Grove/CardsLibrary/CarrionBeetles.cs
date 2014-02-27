namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class CarrionBeetles : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TargetingRule(new EffectRankBy(c => -c.Score, ControlledBy.Opponent));
          });
    }
  }
}