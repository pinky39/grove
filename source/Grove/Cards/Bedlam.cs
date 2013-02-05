namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Modifiers;

  public class Bedlam : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Bedlam")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment")
        .Text("Creatures can't block.")
        .FlavorText("Sometimes quantity, in the absence of quality, is good enough.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ContinuousEffect(p =>
          {
            p.CardFilter = (card, source) => card.Is().Creature;
            p.Modifier = () => new AddStaticAbility(Static.CannotBlock);
          });
    }
  }
}