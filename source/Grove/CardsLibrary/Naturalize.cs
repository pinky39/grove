namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Naturalize : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Naturalize")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text("Destroy target artifact or enchantment.")
        .FlavorText("\"When your cities and trinkets crumble, only nature will remain.\"{EOL}—Garruk Wildspeaker")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Artifact || card.Is().Enchantment)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Destroy));
          });
    }
  }
}