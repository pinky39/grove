namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class PlanarBirth : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Planar Birth")
        .ManaCost("{1}{W}")
        .Type("Sorcery")
        .Text("Return all basic land cards from all graveyards to the battlefield tapped under their owners' control.")
        .FlavorText("From womb of nothingness sprang this place of beauty, purity, and hope realized.")
        .Cast(p =>
          {
            p.TimingRule(new SecondMain());

            p.Effect = () => new PutAllCardsFromGraveyardToBattlefield(
              c => c.Is().BasicLand, c => c.Tap(), eachPlayer: true);
          });
    }
  }
}