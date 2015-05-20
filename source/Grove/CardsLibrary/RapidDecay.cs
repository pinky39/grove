namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class RapidDecay : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rapid Decay")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Exile up to three target cards from a single graveyard.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("The grave robbers arrived the day after the burial. They were a day too late.")
        .Cycling("{2}")
        .Cast(p =>
          {          
            p.Effect = () => new ExileTargets();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Card().In.Graveyard(),
              trg => {                
                trg.MinCount = 0;
                trg.MaxCount = 3;
              });
            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
          });
    }
  }
}