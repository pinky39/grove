namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;

  public class Stupor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Stupor")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Target opponent discards a card at random, then discards a card.")
        .FlavorText("There are medicines for all afflictions but idleness.{EOL}—Suq'Ata saying")
        .Cast(p =>
          {
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<OpponentDiscardsCards>(e =>
              {
                e.RandomCount = 1;
                e.SelectedCount = 1;
              });
          });
    }
  }
}