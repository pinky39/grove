namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;

  public class TemurBanner : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Temur Banner")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{T}: Add {G}, {U}, or {R} to your mana pool.{EOL}{G}{U}{R}, {T}, Sacrifice Temur Banner: Draw a card.")
        .FlavorText("Savagery to survive, courage to triumph.")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {G}, {U}, or {R} to your mana pool.";
          p.ManaAmount(Mana.Colored(isRed: true, isGreen: true, isBlue: true));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{G}{U}{R}, {T}, Sacrifice Temur Banner: Draw a card.";
          p.Cost = new AggregateCost(
            new PayMana("{G}{U}{R}".Parse()),
            new Tap(),
            new Sacrifice());
          p.Effect = () => new DrawCards(1);
        });
    }
  }
}
