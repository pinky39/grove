namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.CastingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class TimeSpiral : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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

            p.TimingRule(new FirstMain());

            p.TimingRule(new ControllerHandCountIs(maxCount: 3));
            p.TimingRule(new OpponentHandCountIs(minCount: 2));
          });
    }
  }
}