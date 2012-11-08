namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class Exhume : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Exhume")
        .ManaCost("{1}{B}")
        .Type("Sorcery")
        .Text("Each player puts a creature card from his or her graveyard onto the battlefield.")
        .FlavorText("'Death—an outmoded concept. We sleep, and we change.'{EOL}—Sitrik, birth priest")
        .Timing(All(Timings.MainPhases(), Timings.HasCardsInGraveyard(card => card.Is().Creature)))
        .Effect<EachPlayerReturnsCardFromGraveyardToBattlefield>();
    }
  }
}