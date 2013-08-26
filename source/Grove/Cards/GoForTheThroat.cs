namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class GoForTheThroat : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Go for the Throat")
        .ManaCost("{1}{B}")
        .Type("Instant")
        .Text("Destroy target nonartifact creature.")
        .FlavorText("Having flesh is increasingly a liability on Mirrodin.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.Is().Artifact)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule());
          });
    }
  }
}