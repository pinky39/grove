namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class FleshToDust : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Flesh to Dust")
                .ManaCost("{3}{B}{B}")
                .Type("Instant")
                .Text("Destroy target creature. It can't be regenerated.")
                .FlavorText("\"Pain is temporary. So is life.\"{EOL}—Liliana Vess")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);
                    p.TargetSelector.AddEffect(trg => trg
                      .Is.Creature()
                      .On.Battlefield());

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new OnFirstMain());
                });
        }
    }
}
