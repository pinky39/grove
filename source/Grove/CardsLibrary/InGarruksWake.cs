namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class InGarruksWake : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("In Garruk's Wake")
        .ManaCost("{7}{B}{B}")
        .Type("Sorcery")
        .Text("Destroy all creatures you don't control and all planeswalkers you don't control.")
        .FlavorText("Beyond pain, beyond obsession and wild despair, there lies a place of twisted power only the most tormented souls can reach.")
        .Cast(p =>
        {
          p.Effect = () => new DestroyAllPermanents(
            filter: (c, ctx) => (c.Is().Creature || c.Is("Planeswalker")) && c.Controller == ctx.Opponent);
        });
    }
  }
}
