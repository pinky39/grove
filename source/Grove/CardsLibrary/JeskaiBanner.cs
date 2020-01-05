namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;
  using AI.TimingRules;
  using AI;

  public class JeskaiBanner : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jeskai Banner")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{T}: Add {U}, {R}, or {W} to your mana pool.{EOL}{U}{R}{W}, {T}, Sacrifice Jeskai Banner: Draw a card.")
        .FlavorText("Discipline to persevere, insight to discover.")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {U}, {R}, or {W} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlue: true, isRed: true, isWhite: true));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{U}{R}{W}, {T}, Sacrifice Jeskai Banner: Draw a card.";
          
          p.Cost = new AggregateCost(
            new PayMana("{U}{R}{W}".Parse()),
            new Tap(),
            new Sacrifice());
          
          p.Effect = () => new DrawCards(1);

          p.TimingRule(new Any(
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));
        })
        // TODO scoring should depend on number of lands on battlefield
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2]);      
    }
  }
}
