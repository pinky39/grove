namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Modifiers;

  public class BackToBasics : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Back to Basics")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("Nonbasic lands don't untap during their controllers' untap steps.")
        .FlavorText(
          "A ruler wears a crown while the rest of us wear hats, but which would you rather have when it's raining?")
        .Cast(p => p.TimingRule(new SecondMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddStaticAbility(Static.DoesNotUntap);
            p.CardFilter = (card, source) => card.Is().NonBasicLand;
          });
    }
  }
}