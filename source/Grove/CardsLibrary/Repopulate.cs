namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Repopulate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Repopulate")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text(
          "Shuffle all creature cards from target player's graveyard into that player's library.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ShuffleTargetGraveyardIntoLibrary(c => c.Is().Creature);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectAnyPlayer());
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}