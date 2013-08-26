namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Scrap : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scrap")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Destroy target artifact.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());
            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule());
          });
    }
  }
}