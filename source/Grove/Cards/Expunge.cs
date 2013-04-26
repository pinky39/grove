namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Expunge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Expunge")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text(
          "Destroy target nonartifact, nonblack creature. It can't be regenerated.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black) && !c.Is().Artifact)
              .On.Battlefield());

            p.TargetingRule(new Destroy());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}