namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class Restock : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Restock")
        .ManaCost("{3}{G}{G}")
        .Type("Sorcery")
        .Text("Return two target cards from your graveyard to your hand. Exile Restock.")
        .FlavorText("What looked like a retreat was actually a replenishing.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnToHand();

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Card().In.YourGraveyard();                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
            p.AfterResolve = card => card.Exile();
          });
    }
  }
}