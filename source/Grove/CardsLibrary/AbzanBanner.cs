namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;

  public class AbzanBanner : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Abzan Banner")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{T}: Add {W}, {B}, or {G} to your mana pool.{EOL}{W}{B}{G}, {T}, Sacrifice Abzan Banner: Draw a card.")
        .FlavorText("Stone to endure, roots to remember.")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {W}, {B} or {G} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isGreen: true, isWhite: true));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{W}{B}{G}, {T}, Sacrifice Abzan Banner: Draw a card.";
          p.Cost = new AggregateCost(
            new PayMana("{W}{B}{G}".Parse()),
            new Tap(),
            new Sacrifice());
          p.Effect = () => new DrawCards(1);
        });
    }
  }
}
