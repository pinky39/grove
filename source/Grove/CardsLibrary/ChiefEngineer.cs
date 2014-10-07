namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class ChiefEngineer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chief Engineer")
        .ManaCost("{1}{U}")
        .Type("Creature — Vedalken Artificer")
        .Text("Artifact spells you cast have convoke. {I}(Your creatures can help cast those spells. Each creature you tap while casting an artifact spell pays {1} for or one mana of that creature's color.){/I}")
        .FlavorText("An eye for detail, a mind for numbers, a soul of clockwork.")
        .Power(1)
        .Toughness(3)
        .ContinuousEffect(p =>
        {
          p.ApplyOnlyToPermaments = false;
          p.CardFilter = (card, effect) => card.Is().Artifact && card.Owner == effect.Source.Controller;
          p.Modifier = () => new AddStaticAbility(Static.Convoke);
        });
    }
  }
}
