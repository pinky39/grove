namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;

  public class MarduBanner : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mardu Banner")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{T}: Add {R}, {W}, or {B} to your mana pool.{EOL}{R}{W}{B}, {T}, Sacrifice Mardu Banner: Draw a card.")
        .FlavorText("Speed to strike, fury to smash.")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {R}, {W}, or {B} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isRed: true, isWhite: true));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{R}{W}{B}, {T}, Sacrifice Mardu Banner: Draw a card.";
          p.Cost = new AggregateCost(
            new PayMana("{R}{W}{B}".Parse()),
            new Tap(),
            new Sacrifice());
          p.Effect = () => new DrawCards(1);
        });
    }
  }
}
