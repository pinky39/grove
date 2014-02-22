namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class TimeSpiral : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Time Spiral")
        .ManaCost("{4}{U}{U}")
        .Type("Sorcery")
        .Text(
          "Exile Time Spiral. Each player shuffles his or her graveyard and hand into his or her library, then draws seven cards. You untap up to six lands.")
        .Cast(p =>
          {
            p.Rule = new Sorcery(c => c.Exile());
            p.Effect = () => new CompoundEffect(
              new EachPlayerShufflesHandAndGraveyardIntoLibraryAndDrawsCards(7),
              new UntapSelectedPermanents(0, 6, c => c.Is().Land));

            p.TimingRule(new OnFirstMain());

            p.TimingRule(new WhenYourHandCountIs(maxCount: 3));
            p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
          });
    }
  }
}