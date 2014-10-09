namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class MindSculpt : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mind Sculpt")
        .ManaCost("{1}{U}")
        .Type("Sorcery")
        .Text("Target opponent puts the top seven cards of his or her library into his or her graveyard.")
        .FlavorText("\"Your mind was a curious mix of madness and genius. I just took away the genius.\"{EOL}—Jace Beleren")
        .Cast(p =>
        {
          p.Effect = () => new OpponentPutsTopCardsFromLibraryToGraveyard(7);;
        });
    }
  }
}
