namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class DayOfJudgment : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Day of Judgment")
        .ManaCost("{2}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all creatures.")
        .Timing(Timings.Steps(Step.FirstMain))
        .Category(EffectCategories.Destruction)
        .Effect<DestroyPermanents>((e, _) => e.Filter = (card) => card.Is().Creature);
    }
  }
}