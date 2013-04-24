namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class TimeWarp : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Time Warp")
        .ManaCost("{3}{U}{U}")
        .Type("Sorcery")
        .Text("Target player takes an extra turn after this one.")
        .FlavorText("Just when you thought you'd survived the first wave.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerTakesExtraTurns(1);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TimingRule(new SecondMain());
            p.TargetingRule(new SpellOwner());
          });
    }
  }
}