namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Scrap : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Scrap")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Destroy target artifact.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());
            p.TargetingRule(new Destroy());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}