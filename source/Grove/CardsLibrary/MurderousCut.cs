namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class MurderousCut : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Murderous Cut")
        .ManaCost("{4}{B}")
        .Type("Instant")
        .Text("{Delve}{I}(Each card you exile from your graveyard while casting this spell pays for {1}.){/I}{EOL}Destroy target creature.")
        .FlavorText("Got a diving lesson{EOL}—Sultai expression meaning{EOL}\"was fed to the crocodiles\"")
        .SimpleAbilities(Static.Delve)
        .Cast(p =>
        {
          p.Effect = () => new DestroyTargetPermanents();

          p.TargetSelector.AddEffect(trg => trg
            .Is.Creature()
            .On.Battlefield());

          p.TargetingRule(new EffectDestroy());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy, EffectTag.CreaturesOnly));
        });
    }
  }
}
