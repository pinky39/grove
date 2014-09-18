namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class Restock : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Restock")
              .ManaCost("{3}{G}{G}")
              .Type("Sorcery")
              .Text("Return two target cards from your graveyard to your hand. Exile Restock.")
              .FlavorText("What looked like a retreat was actually a replenishing.")
              .Cast(p =>
              {
                  p.Effect = () => new ReturnToHand();

                  p.TargetSelector.AddEffect(trg =>
                  {
                      trg.In.YourGraveyard();
                      trg.MinCount = 2;
                      trg.MaxCount = 2;
                  });

                  p.TargetingRule(new EffectBounce());
                  p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Bounce));

                  p.PutToZoneAfterResolve = card => card.Exile();
              });
        }
    }
}
