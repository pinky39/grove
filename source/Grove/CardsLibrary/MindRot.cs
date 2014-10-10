namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class MindRot : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mind Rot")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Target player discards two cards.")
        .FlavorText("\"It saddens me to lose a source of inspiration. This one seemed especially promising.\"—Ashiok")
        .Cast(p =>
        {
          p.Effect = () => new DiscardCards(2);
          p.TargetSelector.AddEffect(s => s.Is.Player());
          p.TargetingRule(new EffectOpponent());
        });
    }
  }
}
