namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class Divination : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Divination")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text("Draw two cards.")
        .FlavorText("Even the House of Galan, who takes the most scholarly approach to the mystic traditions, has resorted to exploring more primitive methods in Avacyn's absence.")
        .Timing(Timings.FirstMain())
        .Effect<DrawCards>((e, _) => { e.DrawCount = 2; });
    }
  }
}