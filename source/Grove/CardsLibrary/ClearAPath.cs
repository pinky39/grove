namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class ClearAPath : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Clear a Path")
                .ManaCost("{R}")
                .Type("Sorcery")
                .Text("Destroy target creature with defender.")
                .FlavorText("\"Why do guards always look surprised when we bash them?\" asked Ruric.{EOL}\"I think they expect a bribe,\" said Thar.")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();

                    p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Creature && c.Has().Defender).On.Battlefield());

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new WhenPermanentCountIs(c => c.Is().Creature && c.Has().Defender));
                });
        }
    }
}
