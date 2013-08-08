namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Evacuation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Evacuation")
        .ManaCost("{3}{U}{U}")
        .Type("Instant")
        .Text("Return all creatures to their owners' hands.")
        .FlavorText("The first step of every exodus is from the blood and the fire onto the trail.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnAllPermanentsToHand(c => c.Is().Creature);
            p.TimingRule(new BounceAll());
          });
    }
  }
}