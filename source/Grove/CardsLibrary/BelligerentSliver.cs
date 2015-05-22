namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class BelligerentSliver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Belligerent Sliver")
        .ManaCost("{2}{R}")
        .Type("Creature — Sliver")
        .Text("Sliver creatures you control have \"This creature can't be blocked except by two or more creatures.\"")
        .FlavorText("\"The slivers became adept at provoking a fear response in other species.\"{EOL}—Hastric, Thunian scout ")
        .Power(2)
        .Toughness(2)
        .ContinuousEffect(p =>
        {
          p.Selector = (card, ctx) => card.Is("Sliver") && card.Controller == ctx.You;
          p.Modifier = () => new SetMinBlockerCount(2);          
        });
    }
  }
}
