namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class DayOfJudgment : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Day of Judgment")
        .ManaCost("{2}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all creatures.")
        .Timing(Timings.SecondMain())
        .Category(EffectCategories.Destruction)
        .Effect<DestroyAllPermanents>(e => e.Filter = (card) => card.Is().Creature);
    }
  }
}