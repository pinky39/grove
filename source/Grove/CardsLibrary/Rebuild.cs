namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class Rebuild : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rebuild")
        .ManaCost("{2}{U}")
        .Type("Instant")
        .Text("Return all artifacts to their owners' hands.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ReturnAllPermanentsToHand(c => c.Is().Artifact);

            p.TimingRule(new Any(              
              new AfterOpponentDeclaresAttackers(),
              new OnEndOfOpponentsTurn()));
          });
    }
  }
}