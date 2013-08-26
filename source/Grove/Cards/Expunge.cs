namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Expunge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Expunge")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text(
          "Destroy target nonartifact, nonblack creature. It can't be regenerated.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && !c.HasColor(CardColor.Black) && !c.Is().Artifact)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule());
          });
    }
  }
}