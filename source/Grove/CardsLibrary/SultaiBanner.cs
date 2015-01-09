namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;

  public class SultaiBanner : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sultai Banner")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{T}: Add {B}, {G}, or {U} to your mana pool.{EOL}{B}{G}{U}, {T}, Sacrifice Sultai Banner: Draw a card.")
        .FlavorText("Power to dominate, cruelty to rule.")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {B}, {G}, or {U} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isGreen: true, isBlue: true));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{B}{G}{U}, {T}, Sacrifice Sultai Banner: Draw a card.";
          p.Cost = new AggregateCost(
            new PayMana("{B}{G}{U}".Parse(), ManaUsage.Abilities),
            new Tap(),
            new Sacrifice());
          p.Effect = () => new DrawCards(1);
        });
    }
  }
}
