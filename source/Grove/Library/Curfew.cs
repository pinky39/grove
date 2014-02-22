namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class Curfew : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Curfew")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Each player returns a creature he or she controls to its owner's hand.")
        .FlavorText(". . . But I'm not tired'")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerReturnsCardsToHand(
              minCount: 1,
              maxCount: 1,
              zone: Zone.Battlefield,
              filter: c => c.Is().Creature,
              aiOrdersByDescendingScore: false,
              text: "Select creature to return to hand"
              );

            p.TimingRule(new NonTargetRemovalTimingRule(1));
          });
    }
  }
}