namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;

  public class BackToBasics : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Back to Basics")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("Nonbasic lands don't untap during their controllers' untap steps.")
        .FlavorText(
          "'A ruler wears a crown while the rest of us wear hats, but which would you rather have when it's raining?'{EOL}—Barrin, Principia")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddStaticAbility>(
                (m, _) => m.StaticAbility = Static.DoesNotUntap);
              e.CardFilter = (card, source) => card.Is().NonBasicLand;
            })
        );
    }
  }
}