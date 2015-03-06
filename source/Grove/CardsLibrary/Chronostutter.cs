namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Chronostutter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chronostutter")
        .ManaCost("{5}{U}")
        .Type("Instant")
        .Text("Put target creature into its owner's library second from the top.")
        .FlavorText("Timing is everything.")
        .Cast(p =>
        {
          p.Text = "Put target creature into its owner's library second from the top.";

          p.Effect = () => new PutTargetsIntoLibraryAtPosition(positionFromTheTop: 1);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectPutOnTopOfLibrary());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.CreaturesOnly));
        });
    }
  }
}
