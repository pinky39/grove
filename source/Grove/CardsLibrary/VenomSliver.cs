namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class VenomSliver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Venom Sliver")
        .ManaCost("{1}{G}")
        .Type("Creature — Sliver")
        .Text("Sliver creatures you control have deathtouch.{I}(Any amount of damage a creature with deathtouch deals to a creature is enough to destroy it.){/I}")
        .FlavorText("\"We attacked with arrows dipped in poison. The slivers that did not die began to change.\"{EOL}—Hastric, Thunian scout")
        .Power(1)
        .Toughness(1)
        .ContinuousEffect(p =>
        {
          p.CardFilter = (card, effect) => card.Is("Sliver") && card.Controller == effect.Source.Controller;
          p.Modifier = () => new AddStaticAbility(Static.Deathtouch);
        });
    }
  }
}
