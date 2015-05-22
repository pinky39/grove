namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class Bedlam : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bedlam")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment")
        .Text("Creatures can't block.")
        .FlavorText("Sometimes quantity, in the absence of quality, is good enough.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ContinuousEffect(p =>
          {
            p.Selector = (card, ctx) => card.Is().Creature;
            p.Modifier = () => new AddStaticAbility(Static.CannotBlock);
          });
    }
  }
}