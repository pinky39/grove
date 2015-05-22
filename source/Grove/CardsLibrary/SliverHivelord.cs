namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class SliverHivelord : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sliver Hivelord")
        .ManaCost("{W}{U}{B}{R}{G}")
        .Type("Legendary Creature — Sliver")
        .Text("Sliver creatures you control have indestructible. {I}(Damage and effects that say \"destroy\" don't destroy them.){/I}")
        .FlavorText("\"This is the source, the line unbroken since the calamity that brought such monsters to our shores.\"{EOL}—Hastric, Thunian scout")
        .Power(5)
        .Toughness(5)
        .ContinuousEffect(p =>
        {
          p.Selector = (card, ctx) => card.Is("Sliver") && card.Controller == ctx.You;
          p.Modifier = () => new AddStaticAbility(Static.Indestructible);
        });
    }
  }
}
