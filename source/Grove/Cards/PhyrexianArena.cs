namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class PhyrexianArena : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Phyrexian Arena")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("At the beginning of your upkeep, you draw a card and you lose 1 life.")
        .FlavorText("An audience of one with the malice of thousands.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, you draw a card and you lose 1 life.",
            C.Trigger<AtBegginingOfStep>((t, _) => { t.Step = Step.Upkeep; }),
            C.Effect<DrawCards>(e =>
              {
                e.DrawCount = 1;
                e.Lifeloss = 1;
              }), triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}