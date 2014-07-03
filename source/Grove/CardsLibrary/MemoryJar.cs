namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class MemoryJar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Memory Jar")
        .ManaCost("{5}")
        .Type("Artifact")
        .Text(
          "{T}, Sacrifice Memory Jar: Each player exiles all cards from his or her hand face down and draws seven cards. At the beginning of the next end step, each player discards his or her hand and returns to his or her hand each card he or she exiled this way.")
        // Ai will have trouble activating this since it cannot see hidden cards, it will seem to ai that activating the jar will be useless, 
        // try to override this by adjusting the score to a very low value, which will cause the discarded cards to contribute
        // enough points so ai will at least activate this when it doesn't have anything better.
        .OverrideScore(p =>
          {
            p.Hand = 5;
            p.Battlefield = 6;
            p.Graveyard = 4;
          })
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}, Sacrifice Memory Jar: Each player exiles all cards from his or her hand face down and draws seven cards. At the beginning of the next end step, each player discards his or her hand and returns to his or her hand each card he or she exiled this way.";

            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new PlayersReplaceTheirHandWithNewOneUntilEot();

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}