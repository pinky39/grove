namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class TimeWarp : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Time Warp")
        .ManaCost("{3}{U}{U}")
        .Type("Sorcery")
        .Text("Target player takes an extra turn after this one.")
        .FlavorText("Just when you thought you'd survived the first wave.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerTakesExtraTurn();
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectYou());
          });
    }
  }
}