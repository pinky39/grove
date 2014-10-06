namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class PillarOfLight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pillar of Light")
        .ManaCost("{2}{W}")
        .Type("Instant")
        .Text("Exile target creature with toughness 4 or greater.")
        .FlavorText("\"The vaulted ceiling of our faith rests upon such pillars.\"—Darugand, banisher priest")
        .Cast(p =>
          {
            p.Effect = () => new ExileTargets();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(card => card.Is().Creature && card.Toughness >= 4).On.Battlefield());

            p.TargetingRule(new EffectExileBattlefield());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Exile, EffectTag.CreaturesOnly));
          });
    }
  }
}