namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class VoidSnare : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Void Snare")
        .ManaCost("{U}")
        .Type("Sorcery")
        .Text("Return target nonland permanent to its owner's hand.")
        .FlavorText("\"I've tried so many variations on how to get rid of annoying things that it's hard to decide which one I like best.\"{EOL}—Ashurel, voidmage")
        .Cast(p =>
        {
          p.Effect = () => new ReturnToHand();
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => !c.Is().Land).On.Battlefield());

          p.TargetingRule(new EffectBounce());
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Bounce));
        });
    }
  }
}
