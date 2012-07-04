namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class Stupor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Stupor")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Target opponent discards a card at random, then discards a card.")
        .FlavorText("There are medicines for all afflictions but idleness.{EOL}—Suq'Ata saying")
        .Effect<OpponentDiscardsCards>((e, _) =>
        {
          e.RandomCount = 1;
          e.SelectedCount = 1;
        })
        .Timing(Timings.FirstMain());
    }
  }
}