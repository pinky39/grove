namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;

  public class BackToBasics : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Back to Basics")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("Nonbasic lands don't untap during their controllers' untap steps.")
        .FlavorText(
          "'A ruler wears a crown while the rest of us wear hats, but which would you rather have when it's raining?'{EOL}—Barrin, Principia")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddStaticAbility>(
                m => m.StaticAbility = Static.DoesNotUntap);
              e.CardFilter = (card, source) => card.Is().NonBasicLand;
            })
        );
    }
  }
}